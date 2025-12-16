using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UserSkill
{
    private readonly Sprite _iconOnButton;
    private readonly ParticleSystem _attackZone;
    protected int[,] Configuration;
    protected int OffsetX;
    protected int OffsetZ;
    protected int Damage;
    protected string Description;

    public UserSkill(Sprite iconOnButton, ParticleSystem attackZone)
    {
        _iconOnButton = iconOnButton != null ? iconOnButton : throw new InvalidOperationException("iconOnButton is null");
        _attackZone = attackZone != null ? attackZone : throw new InvalidOperationException("attackZone is null");
    }

    internal event Action Used;

    internal Sprite IconOnButton => _iconOnButton;
    internal ParticleSystem AttackZone => _attackZone;
    internal int SkillDamage => Damage;
    internal string SkillDescription => Description;

    internal List<LocalPosition> GetSkillCoordinates(LocalPosition position, int minBorderArea, int maxBorderArea)
    {
        List<LocalPosition> coordinates = new();

        for (int i = 0; i < Configuration.GetLength(0); i++)
        {
            for (int j = 0; j < Configuration.GetLength(1); j++)
            {
                if (Configuration[i, j] > 0)
                {
                    int coordinateX = position.PositionX + j + OffsetX;
                    int coordinateZ = position.PositionZ + i + OffsetZ;

                    if (UserUtilities.IsInRangeInt(coordinateX, minBorderArea, maxBorderArea) && UserUtilities.IsInRangeInt(coordinateZ, minBorderArea, maxBorderArea))
                        coordinates.Add(new LocalPosition(coordinateX, coordinateZ));
                }
            }
        }

        return coordinates;
    }

    internal void Use()
    {
        Used?.Invoke();
    }
}