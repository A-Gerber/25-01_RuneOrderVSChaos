using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

public interface IUseableUserSkills
{
    bool TryFindTargetsForStrike(List<LocalPosition> coordinates, out List<Cube> targets);

    List<CellModel> GetCellsForFilling(out List<LocalPosition> cellCoordinates, IReadOnlyList<LocalPosition> skillCoordinates);

    bool TryFindTargetCellsByLines();

    void SetCountTargetDamage(int count);
}
