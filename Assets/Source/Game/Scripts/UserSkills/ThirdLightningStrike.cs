using UnityEngine;

public class ThirdLightningStrike : UserSkill, ISetableInFirstButton
{
    private readonly string _description;
    private readonly int[,] _configuration;
    private readonly int _offset = -2;

    public ThirdLightningStrike(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip) : base(iconOnButton, effect, audioClip)
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

    internal override void Use(Vector3 position)
    {
        Effect.transform.position = position;
        base.Use(position);
    }
}