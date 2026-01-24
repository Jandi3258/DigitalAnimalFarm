namespace BlazorApp1.Models;

public class BigDog : Animal 
{
    public override string Name => "Duży Pies";
    public override string Icon => "🐩";
    public override int ValueInRabbits => 36; // Kosztuje 1 krowę
}