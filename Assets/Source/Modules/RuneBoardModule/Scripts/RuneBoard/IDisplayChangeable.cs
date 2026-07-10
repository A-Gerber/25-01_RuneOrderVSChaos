using System.Collections.Generic;

public interface IDisplayChangeable
{
    public void ChangeRunesDisplay(List<LocalPosition> cubePositions);

    public void DisableAllRunes();
}
