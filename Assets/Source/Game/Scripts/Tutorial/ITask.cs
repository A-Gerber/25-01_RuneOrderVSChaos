using System;

internal interface ITask
{
    event Action Completed;

    void StartTask();

    void Unsubscribe();
}
