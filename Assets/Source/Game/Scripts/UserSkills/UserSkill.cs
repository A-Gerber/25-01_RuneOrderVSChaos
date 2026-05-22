using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UserSkill
{
    protected int[,] Configuration;
    protected int OffsetX;
    protected int OffsetZ;
    protected int Damage;
    protected string Description;

    private readonly Sprite _iconOnButton;
    private readonly ParticleSystem _effect;
    private readonly int _startManaCost;

    private int _increasePerLevel;

    public UserSkill(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip, int manaCost)
    {
        if (manaCost < 0)
            throw new ArgumentOutOfRangeException(nameof(manaCost));

        _iconOnButton = iconOnButton != null ? iconOnButton : throw new InvalidOperationException("iconOnButton is null");
        _effect = effect != null ? effect : throw new InvalidOperationException("attackZone is null");
        AudioClip = audioClip != null ? audioClip : throw new InvalidOperationException("audioClip is null");
        _startManaCost = manaCost;

        _effect.Stop();
    }

    public event Action Used;

    internal AudioClip AudioClip {  get; private set; }
    internal int ManaCost => _startManaCost + _increasePerLevel;
    internal Sprite IconOnButton => _iconOnButton;
    internal int SkillDamage => Damage;
    internal string SkillDescription => Description;

    protected ParticleSystem Effect => _effect;

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

    internal abstract void SetDescriptionLanguage(Languages language);
    internal abstract string GetName();

    internal virtual void Use(Vector3 position)
    {
        _effect.Play();
        Used?.Invoke();
    }

    internal void SetIncrease(int increasePerLevel)
    {
        if (increasePerLevel < 0)
            throw new ArgumentOutOfRangeException(nameof(increasePerLevel));

        _increasePerLevel = increasePerLevel;
    }
}