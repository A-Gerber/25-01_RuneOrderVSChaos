using System;
using System.Collections;
using UnityEngine;

internal class RuneBoardPresenter : MonoBehaviour, IProcessableStep
{
    [SerializeField] private float _delayBeforeStep = 0.35f;

    private RuneBoard _runeBoard;
    private RuneBoardView _view;
    private WaitForSeconds _waitBeforeStep;
    private Coroutine _coroutine;

    private void Awake()
    {
        _waitBeforeStep = new WaitForSeconds(_delayBeforeStep);
    }

    private void OnEnable()
    {
        _runeBoard?.Set(true);
    }

    private void OnDisable()
    {
        _runeBoard?.Set(false);
    }

    public void ProcessStep()
    {
        _coroutine = StartCoroutine(ProcessStepOverTime());
    }

    internal void Initialize(RuneBoard runeBoard, RuneBoardView view)
    {
        if (_runeBoard != null)
        {
            _runeBoard.StartedGame -= OnStartNewLevel;
        }

        _runeBoard = runeBoard ?? throw new ArgumentNullException("runeBoard is null", nameof(runeBoard));
        _view = view != null ? view : throw new ArgumentNullException("view is null", nameof(view));

        if (_runeBoard != null)
        {
            _runeBoard.StartedGame += OnStartNewLevel;
        }
    }

    private void OnStartNewLevel()
    {
        if (!enabled)
            return;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _view.UpdateData(_runeBoard.CurrentLevel, _runeBoard.GameScore);
    }

    private IEnumerator ProcessStepOverTime()
    {
        yield return _waitBeforeStep;
        _runeBoard.ProcessStep();
    }
}
