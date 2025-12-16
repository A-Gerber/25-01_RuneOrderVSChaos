using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

public interface IUseableUserSkills
{
    bool TryFindTargetCellsForStrike(List<LocalPosition> coordinates);

    List<CellModel> GetCellsForFilling(out List<LocalPosition> cellCoordinates, List<LocalPosition> skillCoordinates);

    bool TryFindTargetCellsByLines();

    void SetCountTargetDamage(int count);
}
