using UnityEngine;

public class HorizontalFilling : UserSkill, ISetableInSecondButton
{
    private readonly string _description;
    private readonly int[,] _configuration;
    private readonly int _offsetX = -7;
    private readonly int _offsetZ = 0;

    public HorizontalFilling(Sprite iconOnButton, ParticleSystem attackZone) : base(iconOnButton, attackZone)
    {
        _configuration = new int[,] {
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            };

        Configuration = _configuration;
        OffsetX = _offsetX;
        OffsetZ = _offsetZ;

        _description = "HorizontalFilling";
        Description = _description;
    }
}