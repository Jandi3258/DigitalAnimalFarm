using BlazorApp1.Models;

namespace BlazorApp1.Logic;

public class GameEngine
{
    public Player CurrentPlayer { get; private set; } = new Player();
    private Random _random = new Random();
    
    public void RollDice()
    {
        
        var rollResult = GetDiceResult();
        Console.WriteLine(rollResult);
        ApplyGrowth(rollResult.Die1, rollResult.Die2);
        

        //TODO: Obsługa wilka i lisa (BigDog/SmallDog)
    }

    //TODO: consider not using strings on that
    private (string Die1, string Die2) GetDiceResult()
    {
        string[] orangeDieFaces = 
        {
            "Rabbit", "Rabbit", "Rabbit", "Rabbit", "Rabbit", "Rabbit",
            "Sheep", "Sheep", "Sheep",
            "Pig",
            "Horse",
            "Fox"
        };
        string[] blueDieFaces = 
        {
            "Rabbit", "Rabbit", "Rabbit", "Rabbit", "Rabbit", "Rabbit",
            "Sheep", "Sheep",
            "Pig", "Pig",
            "Cow",
            "Wolf"
        };

        int index1 = _random.Next(orangeDieFaces.Length);
        int index2 = _random.Next(blueDieFaces.Length);

        string result1 = orangeDieFaces[index1];
        string result2 = blueDieFaces[index2];

        return (result1, result2);
    }
    
    public void ApplyGrowth(string die1, string die2)
    {
        // 1. Najpierw zliczamy ile czego wypadło na kostkach.
        // Robimy to ręcznie, bo jest szybciej i czytelniej niż pętle przy intach.
        
        int rolledRabbits = (die1 == "Rabbit" ? 1 : 0) + (die2 == "Rabbit" ? 1 : 0);
        int rolledSheep   = (die1 == "Sheep" ? 1 : 0) + (die2 == "Sheep" ? 1 : 0);
        int rolledPigs    = (die1 == "Pig" ? 1 : 0) + (die2 == "Pig" ? 1 : 0);
        
        // Krowa jest tylko na niebieskiej, Koń tylko na pomarańczowej, 
        // ale dla bezpieczeństwa sprawdzamy obie zmienne:
        int rolledCows    = (die1 == "Cow" ? 1 : 0) + (die2 == "Cow" ? 1 : 0);
        int rolledHorses  = (die1 == "Horse" ? 1 : 0) + (die2 == "Horse" ? 1 : 0);

        // 2. Aktualizujemy stan gracza (dodajemy nowe zwierzęta)
        // Używamy metody pomocniczej CalculateBabies, żeby nie powielać wzoru (x+y)/2
        
        CurrentPlayer.Rabbits += CalculateBabies(CurrentPlayer.Rabbits, rolledRabbits);
        CurrentPlayer.Sheep   += CalculateBabies(CurrentPlayer.Sheep, rolledSheep);
        CurrentPlayer.Pigs    += CalculateBabies(CurrentPlayer.Pigs, rolledPigs);
        CurrentPlayer.Cows    += CalculateBabies(CurrentPlayer.Cows, rolledCows);
        CurrentPlayer.Horses  += CalculateBabies(CurrentPlayer.Horses, rolledHorses);
    }

    // --- METODA POMOCNICZA ---
    private int CalculateBabies(int ownedCount, int rolledCount)
    {
        // Zasada: Zwierzęta rozmnażają się tylko wtedy, gdy wyrzucisz chociaż jedno na kostce.
        if (rolledCount == 0) return 0;

        // Zasada: Za każdą pełną parę (widoczną na kostce + w zagrodzie) dostajesz 1 młode.
        // Np. Masz 1, Wyrzuciłeś 1 -> Razem 2 -> Pary: 1 -> Dostajesz 1.
        int totalVisible = ownedCount + rolledCount;
        return totalVisible / 2; 
    }
    public void Exchange(string fromAnimal, string toAnimal)
    {
        // Logika wymiany, np. 6 królików na 1 owcę
    }
}