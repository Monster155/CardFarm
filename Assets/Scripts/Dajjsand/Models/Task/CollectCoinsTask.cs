using System;

namespace Dajjsand.Models.Task
{
    [Serializable]
    public class CollectCoinsTask : ITask
    {
        public event Action<ITask> OnTaskUpdated;

        public string TaskName;
        public string TaskText;
        public int CoinsCount;

        public string GetTaskText()
        {
            return "Task text";
        }

        public bool IsComplete()
        {
            OnTaskUpdated?.Invoke(this);
            return CoinsCount == 0;
        }

        public ITask Clone()
        {
            return new CollectCoinsTask()
            {
                TaskName = TaskName,
                TaskText = TaskText,
                CoinsCount = CoinsCount
            };
        }
    }
}