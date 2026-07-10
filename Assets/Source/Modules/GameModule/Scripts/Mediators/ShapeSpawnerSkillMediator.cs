using System;
using UnityEngine;

internal class ShapeSpawnerSkillMediator : MonoBehaviour
{
    private IShapeSpawnerSkillContactable _shapeSpawner;
    private ISkillShapeSpawnerContactable _userSkillPerformer;

    internal void Initialize(IShapeSpawnerSkillContactable shapeSpawner, ISkillShapeSpawnerContactable userSkillPerformer)
    {
        if (_userSkillPerformer != null)
            _userSkillPerformer.UsingSkillForShapeSpawner -= (coordinates) => { if (enabled) _shapeSpawner.CreateCubesUsingSkill(coordinates); };

        if (_shapeSpawner != null)
            _shapeSpawner.ReleasedShape -= (cubeCount) => { if (enabled) _userSkillPerformer.RewardWithMana(new CubeManaReward(cubeCount)); };

        _shapeSpawner = shapeSpawner ?? throw new ArgumentNullException("shapeSpawner is null", nameof(shapeSpawner));
        _userSkillPerformer = userSkillPerformer ?? throw new ArgumentNullException("userSkillPerformer is null", nameof(userSkillPerformer));

        if (_userSkillPerformer != null)
            _userSkillPerformer.UsingSkillForShapeSpawner += (coordinates) => { if (enabled) _shapeSpawner.CreateCubesUsingSkill(coordinates); };

        if (_shapeSpawner != null)
            _shapeSpawner.ReleasedShape += (cubeCount) => { if (enabled) _userSkillPerformer.RewardWithMana(new CubeManaReward(cubeCount)); };
    }
}