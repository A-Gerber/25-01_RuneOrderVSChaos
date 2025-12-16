using System;
using UnityEngine;

public class VerticalFilling : UserSkill, ISetableInSecondButton
{
    private readonly string _description;
    private readonly int[,] _configuration;
    private readonly int _offsetX = -1;
    private readonly int _offsetZ = -7;

    public VerticalFilling(Sprite iconOnButton, ParticleSystem attackZone) : base(iconOnButton, attackZone)
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

        _description = "VerticalFilling";
        Description = _description;
    }
}