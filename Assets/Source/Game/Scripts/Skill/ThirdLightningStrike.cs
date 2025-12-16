using UnityEngine;

public class ThirdLightningStrike : UserSkill, ISetableInFirstButton
{
    private readonly string _description;
    private readonly int[,] _configuration;
    private readonly int _offset = -2;

    public ThirdLightningStrike(Sprite iconOnButton, ParticleSystem attackZone) : base(iconOnButton, attackZone)
    {
        _configuration = new int[,] {
           { 0, 1, 1, 1, 0 },
           { 1, 1, 1, 1, 1 },
           { 1, 1, 1, 1, 1 },
           { 1, 1, 1, 1, 1 },
           { 0, 1, 1, 1, 0 }
       };

        Configuration = _configuration;
        OffsetX = _offset;
        OffsetZ = _offset;

        _description = "ThirdLightningStrike";
        Description = _description;
    }
}