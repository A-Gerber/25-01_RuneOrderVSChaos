public static class RayCastController
{
    public static bool CanPerformRayCast { get; private set; } = true;

    public static void SetRayCastEnabled(bool enable)
    {
        CanPerformRayCast = enable;
    }
}