using System;
using System.Collections.Generic;
using UnityEngine;

internal class TaskHandler : MonoBehaviour, ICloseableTutorial
{
    private readonly List<ITask> _tasks = new();

    private int _index = 0;

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    public void CloseTutorial()
    {
        foreach (var task in _tasks)
        {
            task.Unsubscribe();
        }

        Destroy(gameObject);
    }

    internal void Take(List<ITask> movingShapes)
    {
        Unsubscribe();

        if (movingShapes == null)
            throw new ArgumentNullException(nameof(movingShapes));

        if (movingShapes.Count == 0)
            throw new ArgumentException("movingShapes is empty");

        _tasks.AddRange(movingShapes);

        Subscribe();
    }

    internal void StartTask()
    {
        _tasks[_index].StartTask();
    }

    private void OnTaskCompleted()
    {
        if(_index != _tasks.Count - 1)
        {
            _index++;
            StartTask();
        }
        else
        {
            CloseTutorial();
        }
    }

    private void Subscribe()
    {
        if (_tasks.Count != 0)
        {
            foreach (var task in _tasks)
            {
                task.Completed += OnTaskCompleted;
            }
        }
    }

    private void Unsubscribe()
    {
        if (_tasks.Count != 0)
        {
            foreach (var task in _tasks)
            {
                task.Completed -= OnTaskCompleted;
            }
        }
    }
}
