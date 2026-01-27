using BlazorApp1.Models;

namespace BlazorApp1.Logic;

public class FarmerBot
{
    public void ExecuteBestTrades(GameEngine engine)
    {
        Player me = engine.BotPlayer;

        // Bot handluje tylko jeśli jest w stanie coś zyskać
        if (me.Rabbits >= 6 && engine.MainHerd[AnimalType.Sheep] > 0)
        {
            engine.Exchange(AnimalType.Rabbit, AnimalType.Sheep);
        }

        if (me.Sheep >= 2 && engine.MainHerd[AnimalType.Pig] > 0)
        {
            engine.Exchange(AnimalType.Sheep, AnimalType.Pig);
        }

        // Kupno psa dla ochrony
        if (me.Sheep >= 1 && !me.HasSmallDog && engine.MainHerd[AnimalType.SmallDog] > 0)
        {
            engine.Exchange(AnimalType.Sheep, AnimalType.SmallDog);
        }
    }
}