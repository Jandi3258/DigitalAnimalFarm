public class FarmState
{
    public int Kroliki { get; set; } = 0;
    public int Owce { get; set; } = 0;
    public int Swinie { get; set; } = 0;
    public int Krowy { get; set; } = 0;
    public int Konie { get; set; } = 0;
    public bool MalyPies { get; set; } = false;
    public bool DuzyPies { get; set; } = false;

    public bool IsWinner => Kroliki > 0 && Owce > 0 && Swinie > 0 && Krowy > 0 && Konie > 0;
}