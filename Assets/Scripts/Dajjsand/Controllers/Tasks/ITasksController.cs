using System;
using System.Collections.Generic;
using Dajjsand.Enums;
using Dajjsand.Models.Task;

namespace Dajjsand.Controllers.Tasks
{
    public interface ITasksController
    {
        event Action OnAllTasksFinished;
        List<ITask> GetTasks();
        void UpdateReceivedCards(CardType cardType);
    }
}