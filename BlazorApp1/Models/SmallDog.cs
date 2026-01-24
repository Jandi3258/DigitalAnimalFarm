namespace BlazorApp1.Models;

public class SmallDog : Animal 
{
    public override string Name => "Mały Pies";
    public override string Icon => "🐕";
    public override int ValueInRabbits => 6; // Kosztuje 1 owcę
}