using BlazorApp1.Models;

namespace BlazorApp1.Logic;

public class GameEngine
{
    public Player CurrentPlayer { get; private set; } = new Player();
    public Dictionary<AnimalType, int> MainHerd { get; private set; }
    public (AnimalType Die1, AnimalType Die2)? LastRoll { get; private set; }
    
    private Random _random = new Random();

    public GameEngine()
    {
        MainHerd = new Dictionary<AnimalType, int>
        {
            { AnimalType.Rabbit, 60 }, { AnimalType.Sheep, 24 }, { AnimalType.Pig, 20 },
            { AnimalType.Cow, 12 }, { AnimalType.Horse, 4 },
            { AnimalType.SmallDog, 4 }, { AnimalType.BigDog, 2 }
        };
    }

    public void RollDice()
    {
        LastRoll = GetDiceResult();
        
        if (LastRoll.Value.Die1 == AnimalType.Fox || LastRoll.Value.Die2 == AnimalType.Fox)
            ApplyFox();
        
        if (LastRoll.Value.Die1 == AnimalType.Wolf || LastRoll.Value.Die2 == AnimalType.Wolf)
            ApplyWolf();

        ApplyGrowth(LastRoll.Value.Die1, LastRoll.Value.Die2);
    }

    private void ApplyFox()
    {
        if (CurrentPlayer.HasSmallDog)
        {
            CurrentPlayer.HasSmallDog = false;
            MainHerd[AnimalType.SmallDog]++;
        }
        else
        {
            MainHerd[AnimalType.Rabbit] += CurrentPlayer.Rabbits;
            CurrentPlayer.Rabbits = 0;
        }
    }

    private void ApplyWolf()
    {
        if (CurrentPlayer.HasBigDog)
        {
            CurrentPlayer.HasBigDog = false;
            MainHerd[AnimalType.BigDog]++;
        }
        else
        {
            MainHerd[AnimalType.Sheep] += CurrentPlayer.Sheep;
            MainHerd[AnimalType.Pig] += CurrentPlayer.Pigs;
            MainHerd[AnimalType.Cow] += CurrentPlayer.Cows;
            CurrentPlayer.Sheep = 0;
            CurrentPlayer.Pigs = 0;
            CurrentPlayer.Cows = 0;
        }
    }

    private (AnimalType Die1, AnimalType Die2) GetDiceResult()
    {
        AnimalType[] orangeDie = { 
            AnimalType.Rabbit, AnimalType.Rabbit, AnimalType.Rabbit, AnimalType.Rabbit, AnimalType.Rabbit, AnimalType.Rabbit,
            AnimalType.Sheep, AnimalType.Sheep, AnimalType.Pig, AnimalType.Horse, AnimalType.Fox 
        };
        AnimalType[] blueDie = { 
            AnimalType.Rabbit, AnimalType.Rabbit, AnimalType.Rabbit, AnimalType.Rabbit, AnimalType.Rabbit, AnimalType.Rabbit,
            AnimalType.Sheep, AnimalType.Sheep, AnimalType.Pig, AnimalType.Pig, AnimalType.Cow, AnimalType.Wolf 
        };

        return (orangeDie[_random.Next(orangeDie.Length)], blueDie[_random.Next(blueDie.Length)]);
    }

    public void ApplyGrowth(AnimalType die1, AnimalType die2)
    {
        UpdateAnimal(AnimalType.Rabbit, (die1 == AnimalType.Rabbit ? 1 : 0) + (die2 == AnimalType.Rabbit ? 1 : 0));
        UpdateAnimal(AnimalType.Sheep, (die1 == AnimalType.Sheep ? 1 : 0) + (die2 == AnimalType.Sheep ? 1 : 0));
        UpdateAnimal(AnimalType.Pig, (die1 == AnimalType.Pig ? 1 : 0) + (die2 == AnimalType.Pig ? 1 : 0));
        UpdateAnimal(AnimalType.Cow, (die1 == AnimalType.Cow ? 1 : 0) + (die2 == AnimalType.Cow ? 1 : 0));
        UpdateAnimal(AnimalType.Horse, (die1 == AnimalType.Horse ? 1 : 0) + (die2 == AnimalType.Horse ? 1 : 0));
    }

    private void UpdateAnimal(AnimalType type, int rolledCount)
    {
        if (rolledCount == 0) return;
        int owned = GetPlayerCount(type);
        int babies = (owned + rolledCount) / 2;
        
        int available = Math.Min(babies, MainHerd[type]);
        AddAnimalToPlayer(type, available);
        MainHerd[type] -= available;
    }

    // --- NOWA LOGIKA WYMIANY ---
    public void Exchange(AnimalType from, AnimalType to)
    {
        int fromVal = GetAnimalValue(from);
        int toVal = GetAnimalValue(to);

        if (fromVal == 0 || toVal == 0) return;

        // Wymiana "w górę" (np. 6 królików -> 1 owca)
        if (fromVal < toVal)
        {
            int cost = toVal / fromVal;
            if (GetPlayerCount(from) >= cost && MainHerd[to] > 0)
            {
                RemoveAnimal(from, cost);
                AddAnimalToPlayer(to, 1);
                MainHerd[from] += cost;
                MainHerd[to] -= 1;
            }
        }
        // Wymiana "w dół" (np. 1 owca -> 6 królików)
        else
        {
            int yield = fromVal / toVal;
            if (GetPlayerCount(from) >= 1 && MainHerd[to] >= yield)
            {
                RemoveAnimal(from, 1);
                AddAnimalToPlayer(to, yield);
                MainHerd[from] += 1;
                MainHerd[to] -= yield;
            }
        }
    }

    private int GetAnimalValue(AnimalType type) => type switch
    {
        AnimalType.Rabbit => 1,
        AnimalType.Sheep => 6,
        AnimalType.SmallDog => 6,
        AnimalType.Pig => 12,
        AnimalType.Cow => 36,
        AnimalType.BigDog => 36,
        AnimalType.Horse => 72,
        _ => 0
    };

    private int GetPlayerCount(AnimalType type) => type switch {
        AnimalType.Rabbit => CurrentPlayer.Rabbits,
        AnimalType.Sheep => CurrentPlayer.Sheep,
        AnimalType.Pig => CurrentPlayer.Pigs,
        AnimalType.Cow => CurrentPlayer.Cows,
        AnimalType.Horse => CurrentPlayer.Horses,
        AnimalType.SmallDog => CurrentPlayer.HasSmallDog ? 1 : 0,
        AnimalType.BigDog => CurrentPlayer.HasBigDog ? 1 : 0,
        _ => 0
    };

    private void AddAnimalToPlayer(AnimalType type, int count) {
        switch (type) {
            case AnimalType.Rabbit: CurrentPlayer.Rabbits += count; break;
            case AnimalType.Sheep: CurrentPlayer.Sheep += count; break;
            case AnimalType.Pig: CurrentPlayer.Pigs += count; break;
            case AnimalType.Cow: CurrentPlayer.Cows += count; break;
            case AnimalType.Horse: CurrentPlayer.Horses += count; break;
            case AnimalType.SmallDog: CurrentPlayer.HasSmallDog = true; break;
            case AnimalType.BigDog: CurrentPlayer.HasBigDog = true; break;
        }
    }

    private void RemoveAnimal(AnimalType type, int count) {
        switch (type) {
            case AnimalType.Rabbit: CurrentPlayer.Rabbits -= count; break;
            case AnimalType.Sheep: CurrentPlayer.Sheep -= count; break;
            case AnimalType.Pig: CurrentPlayer.Pigs -= count; break;
            case AnimalType.Cow: CurrentPlayer.Cows -= count; break;
            case AnimalType.Horse: CurrentPlayer.Horses -= count; break;
            case AnimalType.SmallDog: CurrentPlayer.HasSmallDog = false; break;
            case AnimalType.BigDog: CurrentPlayer.HasBigDog = false; break;
        }
    }
}