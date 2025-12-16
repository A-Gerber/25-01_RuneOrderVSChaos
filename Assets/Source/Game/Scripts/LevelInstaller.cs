using UnityEngine;

internal class LevelInstaller : MonoBehaviour
{
    [SerializeField] private Factory _factory;
    [SerializeField] private MenuView _menu;
    [SerializeField] private GameView _gameView;

    private void Awake()
    {
        GameModel gameModel = _factory.CreateGameModel();
        _gameView.Initialize(gameModel, _menu);
        _menu.Initialize(gameModel);
        _factory.InitializeUserSkillHandler(_gameView);
    }

    private void Start()
    {
        _gameView.NewGame();
        //_menu.OpenMenu();
    }
}