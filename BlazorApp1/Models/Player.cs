namespace BlazorApp1.Models;

public class Player
{
    public int Rabbits { get; set; }
    public int Sheep { get; set; }
    public int Pigs { get; set; }
    public int Cows { get; set; }
    public int Horses { get; set; }
    public bool HasSmallDog { get; set; }
    public bool HasBigDog { get; set; }

    public bool HasWon() => 
        Rabbits > 0 && Sheep > 0 && Pigs > 0 && Cows > 0 && Horses > 0;
}