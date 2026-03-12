using System;
using UnityEngine;

public class VerticalFilling : UserSkill, ISetableInSecondButton
{
    private readonly string _description;
    private readonly int[,] _configuration;
    private readonly int _offsetX = -1;
    private readonly int _offsetZ = -7;

    public VerticalFilling(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip) : base(iconOnButton, effect, audioClip)
    {
        _configuration = new int[,] {
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1}
       };

        Configuration = _configuration;
        OffsetX = _offsetX;
        OffsetZ = _offsetZ;

        _description = "<color=#FFC300>Creating Runes III\n<color=white>Creates runes in the form of <color=#FFC300>three vertical lines";
        Description = _description;
    }

    internal override void Use(Vector3 position)
    {
        Effect.transform.position = position;
        base.Use(position);
    }
}