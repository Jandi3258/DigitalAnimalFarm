using BlazorApp1.Models;
using System.Linq;

namespace BlazorApp1.Logic;

public class GameEngine
{
    // Gracze
    public Player HumanPlayer { get; } = new Player();
    public Player BotPlayer { get; } = new Player();
    public Player CurrentPlayer { get; private set; } 

    // Stan gry
    public Dictionary<AnimalType, int> MainHerd { get; private set; }
    public (AnimalType Die1, AnimalType Die2)? LastRoll { get; private set; }
    public Action<string>? OnGameMessage; 

    private readonly Random _random = new();

    // Mapowanie właściwości gracza
    private readonly Dictionary<AnimalType, (Func<Player, int> Get, Action<Player, int> Add, Action<Player, bool> SetDog)> _playerInventoryMap;

    // Wartości rynkowe zwierząt
    private readonly Dictionary<AnimalType, int> _animalValues = new()
    {
        { AnimalType.Rabbit, 1 }, { AnimalType.Sheep, 6 }, { AnimalType.Pig, 12 },
        { AnimalType.Cow, 36 }, { AnimalType.Horse, 72 }, { AnimalType.SmallDog, 6 }, { AnimalType.BigDog, 36 }
    };
    
    public GameEngine()
    {
        CurrentPlayer = HumanPlayer; 

        
        MainHerd = new Dictionary<AnimalType, int>
        {
            { AnimalType.Rabbit, 60 }, { AnimalType.Sheep, 24 }, { AnimalType.Pig, 20 },
            { AnimalType.Cow, 12 }, { AnimalType.Horse, 4 },
            { AnimalType.SmallDog, 4 }, { AnimalType.BigDog, 2 }
        };

        //Delegaty
        _playerInventoryMap = new()
        {
            { AnimalType.Rabbit, (p => p.Rabbits, (p, v) => p.Rabbits += v, (_, _) => { }) },
            { AnimalType.Sheep, (p => p.Sheep, (p, v) => p.Sheep += v, (_, _) => { }) },
            { AnimalType.Pig, (p => p.Pigs, (p, v) => p.Pigs += v, (_, _) => { }) },
            { AnimalType.Cow, (p => p.Cows, (p, v) => p.Cows += v, (_, _) => { }) },
            { AnimalType.Horse, (p => p.Horses, (p, v) => p.Horses += v, (_, _) => { }) },
            { AnimalType.SmallDog, (p => (p.HasSmallDog ? 1 : 0), (_, _) => { }, (p, v) => p.HasSmallDog = v) },
            { AnimalType.BigDog, (p => (p.HasBigDog ? 1 : 0), (_, _) => { }, (p, v) => p.HasBigDog = v) }
        };
    }
    
    public void EndTurn()
    {
        CurrentPlayer = (CurrentPlayer == HumanPlayer) ? BotPlayer : HumanPlayer;
        string name = (CurrentPlayer == HumanPlayer) ? "Gracza" : "Bota";
        OnGameMessage?.Invoke($"--- Tura {name} ---");
    }

    public void RollDice()
    {
        LastRoll = GetDiceResult();
        var dice = new[] { LastRoll.Value.Die1, LastRoll.Value.Die2 };

        if (dice.Any(d => d == AnimalType.Fox)) ApplyFox();
        if (dice.Any(d => d == AnimalType.Wolf)) ApplyWolf();

        ApplyGrowth(dice);
    }

    private void ApplyFox()
    {
        if (CurrentPlayer.HasSmallDog)
        {
            ModifyPlayerStock(AnimalType.SmallDog, false);
            MainHerd[AnimalType.SmallDog]++;
            OnGameMessage?.Invoke("🦊 Lis zaatakował! Mały pies ratuje stado.");
        }
        else
        {
            int rabbits = CurrentPlayer.Rabbits;
            if (rabbits > 0)
            {
                MainHerd[AnimalType.Rabbit] += rabbits;
                CurrentPlayer.Rabbits = 0;
                OnGameMessage?.Invoke($"🦊 Lis zjadł {rabbits} królików!");
            }
        }
    }

    private void ApplyWolf()
    {
        if (CurrentPlayer.HasBigDog)
        {
            ModifyPlayerStock(AnimalType.BigDog, false);
            MainHerd[AnimalType.BigDog]++;
            OnGameMessage?.Invoke("🐺 Wilk zaatakował! Duży pies broni farmy.");
        }
        else
        {
            var edibleTypes = new[] { AnimalType.Sheep, AnimalType.Pig, AnimalType.Cow };
            bool ateSomething = false;

            foreach (var type in edibleTypes)
            {
                int count = GetPlayerCount(type);
                if (count > 0)
                {
                    MainHerd[type] += count;
                    _playerInventoryMap[type].Add(CurrentPlayer, -count); 
                    ateSomething = true;
                }
            }

            if (ateSomething) 
                OnGameMessage?.Invoke("🐺 Wilk spustoszył stado! Straciłeś owce, świnie i krowy.");
        }
    }

    private (AnimalType, AnimalType) GetDiceResult()
    {
        var orangeDie = Enumerable.Repeat(AnimalType.Rabbit, 6)
            .Concat(Enumerable.Repeat(AnimalType.Sheep, 2))
            .Concat(new[] { AnimalType.Pig, AnimalType.Horse, AnimalType.Fox })
            .ToArray();

        var blueDie = Enumerable.Repeat(AnimalType.Rabbit, 6)
            .Concat(Enumerable.Repeat(AnimalType.Sheep, 2))
            .Concat(Enumerable.Repeat(AnimalType.Pig, 2))
            .Concat(new[] { AnimalType.Cow, AnimalType.Wolf })
            .ToArray();

        return (orangeDie[_random.Next(orangeDie.Length)], blueDie[_random.Next(blueDie.Length)]);
    }

    public void ApplyGrowth(AnimalType[] dice)
    {
        var breedableTypes = _playerInventoryMap.Keys
            .Where(k => k != AnimalType.SmallDog && k != AnimalType.BigDog)
            .ToList();

        foreach (var type in breedableTypes)
        {
            int rolledCount = dice.Count(d => d == type);
            if (rolledCount > 0)
            {
                int owned = GetPlayerCount(type);
                int babies = (owned + rolledCount) / 2;
                
                if (babies > 0)
                {
                    int available = Math.Min(babies, MainHerd[type]);
                    if (available > 0)
                    {
                        ModifyPlayerStock(type, available);
                        MainHerd[type] -= available;
                        OnGameMessage?.Invoke($"Urodziło się: {available}x {type}");
                    }
                }
            }
        }
    }

    public void Exchange(AnimalType from, AnimalType to)
    {
        int fromVal = GetAnimalValue(from);
        int toVal = GetAnimalValue(to);

        if (fromVal == 0 || toVal == 0) return;

        if (fromVal < toVal) // W górę
        {
            int cost = toVal / fromVal;
            if (GetPlayerCount(from) >= cost && MainHerd[to] > 0)
            {
                ModifyPlayerStock(from, -cost);
                ModifyPlayerStock(to, 1);
                MainHerd[from] += cost;
                MainHerd[to] -= 1;
                OnGameMessage?.Invoke($"Handel: {cost} {from} -> 1 {to}");
            }
        }
        else // W dół
        {
            int yield = fromVal / toVal;
            if (GetPlayerCount(from) >= 1 && MainHerd[to] >= yield)
            {
                ModifyPlayerStock(from, -1);
                ModifyPlayerStock(to, yield);
                MainHerd[from] += 1;
                MainHerd[to] -= yield;
                OnGameMessage?.Invoke($"Handel: 1 {from} -> {yield} {to}");
            }
        }
    }

    private int GetAnimalValue(AnimalType type) => _animalValues.GetValueOrDefault(type, 0);

    private int GetPlayerCount(AnimalType type) => 
        _playerInventoryMap.TryGetValue(type, out var funcs) ? funcs.Get(CurrentPlayer) : 0;

    private void ModifyPlayerStock(AnimalType type, object value)
    {
        if (!_playerInventoryMap.TryGetValue(type, out var funcs)) return;

        if (value is int intVal)
        {
            if (type == AnimalType.SmallDog || type == AnimalType.BigDog)
            {
                if (intVal != 0) funcs.SetDog(CurrentPlayer, intVal > 0);
            }
            else
            {
                funcs.Add(CurrentPlayer, intVal);
            }
        }
        else if (value is bool boolVal)
        {
            funcs.SetDog(CurrentPlayer, boolVal);
        }
    }
}