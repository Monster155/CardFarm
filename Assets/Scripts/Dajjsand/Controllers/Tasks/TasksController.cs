using System.Collections.Generic;
using Dajjsand.Factories.LevelConfigFactory;
using Dajjsand.Managers.Save;
using Dajjsand.Models;
using Dajjsand.Models.Task;
using Dajjsand.ScriptableObjects;

namespace Dajjsand.Controllers.Tasks
{
    public class TasksController : ITasksController
    {
        private ILevelConfigFactory _levelConfigFactory;
        private ISaveManager _saveManager;

        private LevelConfig _currentLevelConfig;

        public TasksController(ILevelConfigFactory levelConfigFactory, ISaveManager saveManager)
        {
            _levelConfigFactory = levelConfigFactory;
            _levelConfigFactory.OnLoadComplete += LevelConfigFactory_OnLoadComplete;
            
            _saveManager = saveManager;
        }

        ~TasksController()
        {
            _levelConfigFactory.OnLoadComplete -= LevelConfigFactory_OnLoadComplete;
        }

        private void LevelConfigFactory_OnLoadComplete()
        {
            _currentLevelConfig = _levelConfigFactory.GetLevelConfig(_saveManager.GetCurrentLevelIndex());
        }

        public List<ITask> GetTasks() => _currentLevelConfig._tasksToCompleteLevel._tasks;
    }
}