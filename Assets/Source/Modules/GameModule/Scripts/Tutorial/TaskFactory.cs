using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskFactory : MonoBehaviour
{
    [SerializeField] private RectTransform _taskContainer;
    [SerializeField] private TaskHandler _taskHandlerPrefab;
    [SerializeField] private UserSkillScreen _userSkillScreen;
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

    private IReportableOnRelease _spawner;
    private bool _isDesktop;

    internal TaskHandler Create(IReportableOnUsedSkill userSkillPerformer)
    {
        TaskHandler taskHandler = Instantiate(_taskHandlerPrefab, _taskContainer);

        taskHandler.Take(CreateTasks(taskHandler, userSkillPerformer));
        taskHandler.StartTask();

        return taskHandler;
    }

    internal void Initialize(IReportableOnRelease shapePresenterSpawner)
    {
        _spawner = shapePresenterSpawner ?? throw new ArgumentNullException("shapePresenterSpawner is null", nameof(shapePresenterSpawner));
    }

    internal void Set(bool isDesktop)
    {
        _isDesktop = isDesktop;
    }

    private List<ITask> CreateTasks(TaskHandler taskHandler, IReportableOnUsedSkill userSkillPerformer)
    {
        List<ITask> tasks = new()
        {
            new TaskOfGreeting(_greetingScreen,taskHandler),
            new TaskOfOverview(_screenOfOverview),
            CreateThirdTask(),
            new TaskOfUsingSkill(_screenOnUsingSkill,userSkillPerformer,_arrowOfUsingSkill),
            new TaskOfOpenSkillMenu(_screenOnOpenSkillMenu,_userSkillScreen,_arrowOfnOpenSkillMenu),
            new TaskOfViewingSkillsMenu(_screenOnViewingSkillsMenu)
        };

        return tasks;
    }


    private TaskOfMovingShape CreateThirdTask()
    {
        TaskOfMovingShape secondTask;

        if (_isDesktop)
            secondTask = new TaskOfMovingShape(_desktopScreenOnMovingShape, _spawner, _arrowsOfMovingShape);
        else
            secondTask = new TaskOfMovingShape(_mobileScreenOnMovingShape, _spawner, _arrowsOfMovingShape);

        return secondTask;
    }
}
