namespace BlazorApp1.Models;

public class Pig : Animal 
{
    public override string Name => "Świnia";
    public override string Icon => "🐷";
    public override int ValueInRabbits => 12; // Warta 2 owce
}