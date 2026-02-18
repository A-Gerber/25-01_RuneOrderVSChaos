using UnityEngine;

public class FourthLightningStrike : UserSkill, ISetableInFirstButton
{
    private readonly string _description;
    private readonly int[,] _configuration;
    private readonly int _offset = -3;

    public FourthLightningStrike(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip) : base(iconOnButton, effect, audioClip)
    {
        _configuration = new int[,] {
                { 0, 0, 1, 1, 1, 0, 0 },
                { 0, 1, 1, 1, 1, 1, 0 },
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1 },
                { 0, 1, 1, 1, 1, 1, 0 },
                { 0, 0, 1, 1, 1, 0, 0 }
            };

        Configuration = _configuration;
        OffsetX = _offset;
        OffsetZ = _offset;

        _description = "FourthLightningStrike";
        Description = _description;
    }

    internal override void Use(Vector3 position)
    {
        Effect.transform.position = position;
        base.Use(position);
    }
}