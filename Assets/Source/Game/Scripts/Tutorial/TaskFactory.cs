using System.Collections.Generic;
using UnityEngine;
using YG;

internal class TaskFactory : MonoBehaviour
{
    [SerializeField] private ShapePresenterSpawner _spawner;
    [SerializeField] private GameView _gameView;
    [SerializeField] private RectTransform _taskContainer;
    [SerializeField] private TaskHandler _taskHandlerPrefab;
    [SerializeField] private List<ParticleSystem> _arrowsOfMovingShape;
    [SerializeField] private ParticleSystem _arrowOfUsingSkill;
    [SerializeField] private ParticleSystem _arrowOfnOpenSkillMenu;

    [Header("Screens")]
    [SerializeField] private ScreenOnGreeting _greetingScreen;
    [SerializeField] private ScreenOfOverview _screenOfOverview;
    [SerializeField] private ScreenOnMovingShape _desktopScreenOnMovingShape;
    [SerializeField] private ScreenOnMovingShape _mobileScreenOnMovingShape;
    [SerializeField] private ScreenOnUsingSkill _screenOnUsingSkill;
    [SerializeField] private ScreenOnOpenSkilslMenu _screenOnOpenSkillMenu;
    [SerializeField] private ScreenOnViewingSkillsMenu _screenOnViewingSkillsMenu;

    internal TaskHandler Create()
    {
        TaskHandler taskHandler = Instantiate(_taskHandlerPrefab, _taskContainer);

        taskHandler.Take(CreateTasks(taskHandler));
        taskHandler.StartTask();

        return taskHandler;
    }

    private List<ITask> CreateTasks(TaskHandler taskHandler)
    {
        List<ITask> tasks = new()
        {
            new TaskOfGreeting(_greetingScreen,taskHandler),
            new TaskOfOverview(_screenOfOverview),
            CreateThirdTask(),
            new TaskOfUsingSkill(_screenOnUsingSkill,_gameView,_arrowOfUsingSkill),
            new TaskOfOpenSkillMenu(_screenOnOpenSkillMenu,_gameView,_arrowOfnOpenSkillMenu),
            new TaskOfViewingSkillsMenu(_screenOnViewingSkillsMenu)
        };

        return tasks;
    }


    private TaskOfMovingShape CreateThirdTask()
    {
        TaskOfMovingShape secondTask;

        if (YG2.envir.isDesktop)
            secondTask = new TaskOfMovingShape(_desktopScreenOnMovingShape, _spawner, _arrowsOfMovingShape);
        else
            secondTask = new TaskOfMovingShape(_mobileScreenOnMovingShape, _spawner, _arrowsOfMovingShape);

        return secondTask;
    }
}