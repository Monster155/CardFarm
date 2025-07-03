using System.Collections.Generic;
using Dajjsand.Models;
using Dajjsand.Models.Task;

namespace Dajjsand.Controllers.Tasks
{
    public interface ITasksController
    {
        List<ITask> GetTasks();
    }
}