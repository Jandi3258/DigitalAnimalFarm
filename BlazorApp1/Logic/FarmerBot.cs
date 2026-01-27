using BlazorApp1.Models;

namespace BlazorApp1.Logic;

public class FarmerBot
{
    public void ExecuteBestTrades(GameEngine engine)
    {
        Player me = engine.BotPlayer;

        if (me.Cows >= 2 && me.Horses == 0 && engine.MainHerd[AnimalType.Horse] > 0)
        {
            engine.Exchange(AnimalType.Cow, AnimalType.Horse);
        }
        if (me.Cows >= 1 && !me.HasBigDog && engine.MainHerd[AnimalType.BigDog] > 0)
        {
            engine.Exchange(AnimalType.Cow, AnimalType.BigDog);
        }

        if (me.Pigs >= 3 && engine.MainHerd[AnimalType.Cow] > 0)
        {
            engine.Exchange(AnimalType.Pig, AnimalType.Cow);
        }

        if (me.Sheep >= 1 && !me.HasSmallDog && engine.MainHerd[AnimalType.SmallDog] > 0)
        {
            engine.Exchange(AnimalType.Sheep, AnimalType.SmallDog);
        }

        if (me.Sheep >= 2 && engine.MainHerd[AnimalType.Pig] > 0)
        {
            engine.Exchange(AnimalType.Sheep, AnimalType.Pig);
        }

        if (me.Rabbits >= 6 && engine.MainHerd[AnimalType.Sheep] > 0)
        {
            engine.Exchange(AnimalType.Rabbit, AnimalType.Sheep);
        }
        
        if (me.Horses > 1)
        {
            engine.Exchange(AnimalType.Horse, AnimalType.Cow);
        }
    }
}