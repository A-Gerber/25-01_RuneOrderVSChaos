using System.Collections.Generic;

public interface ICreateableCubesForArea
{
    void CreateCubesForArea(List<LocalPosition> coordinates, List<CellModel> cells);
}