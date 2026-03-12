using UnityEngine;

public class HorizontalFilling : UserSkill, ISetableInSecondButton
{
    private readonly string _description;
    private readonly int[,] _configuration;
    private readonly int _offsetX = -7;
    private readonly int _offsetZ = 0;

    public HorizontalFilling(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip) : base(iconOnButton, effect, audioClip)
    {
        _configuration = new int[,] {
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            };

        Configuration = _configuration;
        OffsetX = _offsetX;
        OffsetZ = _offsetZ;

        _description = "<color=#FFC300>Creating Runes I\n<color=white>Creates runes in a <color=#FFC300>horizontal line";
        Description = _description;
    }

    internal override void Use(Vector3 position)
    {
        Effect.transform.position = position;
        base.Use(position);
    }
}