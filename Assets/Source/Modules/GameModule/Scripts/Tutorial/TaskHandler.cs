using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskHandler : MonoBehaviour, IClosableTutorial
{
    private readonly List<ITask> _tasks = new();

    private int _index = 0;

    public void CloseTutorial()
    {
        foreach (var task in _tasks)
        {
            task.Completed -= OnTaskCompleted;
            task.Unsubscribe();
        }

        Destroy(gameObject);
    }

    internal void Take(List<ITask> movingShapes)
    {
        if (_tasks.Count != 0)
        {
            foreach (var task in _tasks)
                task.Completed -= OnTaskCompleted;
        }

        if (movingShapes == null)
            throw new ArgumentNullException(nameof(movingShapes));

        if (movingShapes.Count == 0)
            throw new ArgumentException("movingShapes is empty");

        _tasks.AddRange(movingShapes);

        if (_tasks.Count != 0)
        {
            foreach (var task in _tasks)
                task.Completed += OnTaskCompleted;
        }
    }

    internal void StartTask()
    {
        _tasks[_index].StartTask();
    }

    private void OnTaskCompleted()
    {
        if(!enabled)
            return;

        if (_index != _tasks.Count - 1)
        {
            _index++;
            StartTask();
        }
        else
        {
            CloseTutorial();
        }
    }
}
