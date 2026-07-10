using System.Collections.Generic;

public interface IShapeSpawnerSkillContactable : IReportableOnRelease
{
    public void CreateCubesUsingSkill(List<LocalPosition> coordinates);
}
