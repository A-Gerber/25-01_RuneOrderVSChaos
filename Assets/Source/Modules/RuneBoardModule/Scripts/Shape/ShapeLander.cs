using System.Collections.Generic;

internal class ShapeLander
{
    private readonly TransparencyState _opaqueState = new(false);

    internal bool TryLand(List<Cube> cubes)
    {
        foreach (var cube in cubes)
            cube.ChangeState(_opaqueState);

        if (IsFreeSpace(cubes))
        {
            foreach (var cubeModel in cubes)
                cubeModel.Land();

            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsFreeSpace(List<Cube> cubes)
    {
        foreach (var cube in cubes)
        {
            if (cube.TryGetBusyCell())
                return false;
        }

        return true;
    }
}