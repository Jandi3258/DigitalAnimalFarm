namespace BlazorApp1.Models;

public class Horse : Animal 
{
    public override string Name => "Koń";
    public override string Icon => "🐴";
    public override int ValueInRabbits => 72; // Warty 2 krowy
}