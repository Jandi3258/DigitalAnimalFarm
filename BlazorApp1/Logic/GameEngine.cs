namespace BlazorApp1.Logic;
using BlazorApp1.Models;

public class GameEngine
{
    public Dictionary<string, int> MyHerd { get; set; } = new()
    {
        { "Królik", 0 }, { "Owca", 0 }, { "Świnia", 0 }, 
        { "Krowa", 0 }, { "Koń", 0 }, { "Mały Pies", 0 }, { "Duży Pies", 0 }
    };
    public bool HasWon => MyHerd.Where(x => !x.Key.Contains("Pies"))
        .All(x => x.Value > 0);

    public void AddAnimal(string name) => MyHerd[name]++;
}