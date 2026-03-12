using UnityEngine;

internal class ConstantsInstaller : MonoBehaviour
{
    private const int OriginByX = 0;
    private const int OriginByZ = 0;
    private const int AreaSize = 8;

    [Header("GameParameters")]
    [SerializeField] private int _startLevel = 1;
    [SerializeField] private int _lastLevel = 50;
    [SerializeField] private int _startSkillCount = 1;
    [SerializeField] private int _skillCountIncrease = 1;
    [SerializeField] private int _skillPointsInterval = 5;

    [Header("RewardParameters")]
    [SerializeField] private int _rewardForAdvertising = 10;

    [Header("AreaParameters")]
    [SerializeField] private float _flightAltitude = 2f;
    [SerializeField] private Vector2 _minLimitsForLeavingArena = new (-2.5f, -2.2f);
    [SerializeField] private Vector2 _maxLimitsForLeavingArena = new(9.5f, 8.5f);

    private float _cameraHeight;
    private float _cubeSize;
    private float _cellSize;

    internal void SetParameters(float cameraHeight, float cellSize, float cubeSize)
    {
        _cubeSize = cubeSize;
        _cellSize = cellSize;
        _cameraHeight = cameraHeight;
    }

    internal void SetConstants()
    {
        Constants.SetGameParameters(_startLevel, _lastLevel, _startSkillCount, _skillCountIncrease, _skillPointsInterval);
        Constants.SetAreaParameters(OriginByX, OriginByZ, _minLimitsForLeavingArena, _maxLimitsForLeavingArena, AreaSize);
        Constants.SetCameraHeight(_cameraHeight, _flightAltitude);
        Constants.CalculateAreaBorders(_cellSize);
        Constants.SetCubeParameters(_cubeSize);
        Constants.SetRewardForAdvertising(_rewardForAdvertising);
    }
}