using System.Collections.Generic;
using UnityEngine;

public interface IShowableNextSkills
{
    public bool TryGetSkillSprites(out List<Sprite> sprites, int currentLevel);

    public int GetNextThreshold(int currentLevel);
}