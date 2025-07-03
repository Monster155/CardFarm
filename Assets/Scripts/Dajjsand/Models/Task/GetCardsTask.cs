using System;

namespace Dajjsand.Models.Task
{
    [Serializable]
    public class GetCardsTask : ITask
    {
        public string TaskName;
        public string TaskText;
    }
}