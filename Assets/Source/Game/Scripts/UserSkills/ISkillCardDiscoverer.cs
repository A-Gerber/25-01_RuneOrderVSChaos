using System.Collections.Generic;
using UnityEngine;

public interface ISkillCardDiscoverer
{
    int GetNextThreshold(int currentLevel);
    bool TryGetSkillSprites(out List<Sprite> sprites, int currentLevel);
}
