using System.Collections.Generic;
using UnityEngine;

public interface ISkillCardDiscoverer
{
    bool TryGetSkillSprites(out List<Sprite> sprites, int currentLevel);
}