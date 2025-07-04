using System;

namespace Dajjsand.Models.Task
{
    public interface ITask
    {
        event Action<ITask> OnTaskUpdated;
        string GetTaskText();
        bool IsComplete();
        ITask Clone();
    }
}