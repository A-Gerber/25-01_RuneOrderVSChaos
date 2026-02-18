using UnityEngine;

public class SecondLightningStrike : UserSkill, ISetableInFirstButton
{
    private readonly string _description;
    private readonly int[,] _configuration;
    private readonly int _offset = -1;

    public SecondLightningStrike(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip) : base(iconOnButton, effect, audioClip)
    {
        _configuration = new int[,] {
                { 1, 1, 1 },
                { 1, 1, 1 },
                { 1, 1, 1 }
            };

        Configuration = _configuration;
        OffsetX = _offset;
        OffsetZ = _offset;

        _description = "SecondLightningStrike";
        Description = _description;
    }

    internal override void Use(Vector3 position)
    {
        Effect.transform.position = position;
        base.Use(position);
    }
}
