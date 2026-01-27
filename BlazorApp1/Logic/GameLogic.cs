public static class GameLogic
{
    public static int CalculateNewAnimals(int currentCount, int rolledCount)
    {
        if (rolledCount == 0) return 0;
        return (currentCount + rolledCount) / 2;
    }
}