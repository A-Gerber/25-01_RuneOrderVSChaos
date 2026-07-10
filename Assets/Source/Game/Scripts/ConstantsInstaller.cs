using UnityEngine;

internal class ConstantsInstaller : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private CellPresenter _cellViewPrefab;
    [SerializeField] private CubePresenter _cubeViewPrefab;

    [Header("GameParameters")]
    [SerializeField] private int _startLevel = 1;
    [SerializeField] private int _startGameScore = 0;
    [SerializeField] private int _startManaCount = 40;
    [SerializeField] private int _lastLevel = 50;
    [SerializeField] private int _manaCountIncrease = 10;
    [SerializeField] private int _advertisingReward = 100;

    internal int StartGameScore => _startGameScore;
    internal int StartManaCount => _startManaCount;

    internal void SetConstants()
    {
        Constants.SetGameParameters(_startLevel, _lastLevel, _manaCountIncrease, _advertisingReward);
        Constants.CalculateAreaParameters(_cellViewPrefab.transform.localScale.x, _camera.transform.position.y);
        Constants.SetCubeParameters(_cubeViewPrefab.CubeSize);
    }

    internal void SetLanguage(string language)
    {
        switch (language)
        {
            case "ru":
                Constants.SetLanguage(Languages.Russian);
                break;

            case "tr":
                Constants.SetLanguage(Languages.Turkish);
                break;

            default:
                Constants.SetLanguage(Languages.English);
                break;
        }
    }
}