using System;

namespace Dajjsand.Models.Task
{
    [Serializable]
    public class CollectCoinsTask : ITask
    {
        public string TaskName;
        public string TaskText;
        public int CoinsCount;
    }
}