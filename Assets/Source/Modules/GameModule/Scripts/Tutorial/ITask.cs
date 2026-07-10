using System;

internal interface ITask
{
    public event Action Completed;

    public void StartTask();

    public void Unsubscribe();
}
