using UnityEngine;

public class FirstLightningStrike : UserSkill, ISetableInFirstButton
{
    private readonly string _description;
    private readonly int[,] _configuration;
    private readonly int _offset = -1;

    public FirstLightningStrike(Sprite iconOnButton, ParticleSystem attackZone) : base(iconOnButton, attackZone)
    { 
        _configuration = new int[,] {
                { 0, 1, 0 },
                { 1, 1, 1 },
                { 0, 1, 0 }
            };

        Configuration = _configuration;
        OffsetX = _offset;
        OffsetZ = _offset;

        _description = "FirstLightningStrike";
        Description = _description;
    }
}