using System;
using System.Collections.Generic;
using UnityEngine;

internal class TaskOfMovingShape : ITask
{
    private readonly ScreenOnMovingShape _screen;
    private readonly IReportableOnRelease _spawner;
    private readonly List<ParticleSystem> _arrows = new();

    private bool _isPerformed = false;

    public TaskOfMovingShape(ScreenOnMovingShape screen, IReportableOnRelease spawner, List<ParticleSystem> arrows)
    {
        if (arrows == null)
            throw new ArgumentNullException(nameof(arrows));

        if (arrows.Count == 0)
            throw new ArgumentException("arrows is empty");

        _screen = screen != null ? screen : throw new InvalidOperationException("screen is null");
        _spawner = spawner ?? throw new InvalidOperationException("spawner is null");
        _arrows.AddRange(arrows);

        Subscribe();
    }

    public event Action Completed;

    public void StartTask()
    {
        _screen.Open();
        _isPerformed = true;

        foreach (var arrow in _arrows)
            arrow.gameObject.SetActive(true);
    }

    public void Unsubscribe()
    {
        if (_spawner != null)
            _spawner.ReleasedShape -= OnReleaseShape;

        if (_screen != null)
            _screen.ExitButtonClicked -= OnExitButtonClick;

        foreach (var arrow in _arrows)
            arrow.gameObject.SetActive(false);
    }

    private void OnReleaseShape(int count)
    {
        if (_isPerformed)
        {
            _screen.Close();
            _isPerformed = false;
            Completed?.Invoke();

            foreach (var arrow in _arrows)
                arrow.gameObject.SetActive(false);
        }
    }

    private void OnExitButtonClick()
    {
        _screen.Close();
    }

    private void Subscribe()
    {
        if (_spawner != null)
            _spawner.ReleasedShape += OnReleaseShape;

        if (_screen != null)
            _screen.ExitButtonClicked += OnExitButtonClick;
    }
}