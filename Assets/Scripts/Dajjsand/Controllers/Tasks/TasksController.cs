using System;
using System.Collections.Generic;
using Dajjsand.Enums;
using Dajjsand.Factories.LevelConfigFactory;
using Dajjsand.Managers.Save;
using Dajjsand.Models.Task;
using Dajjsand.ScriptableObjects;

namespace Dajjsand.Controllers.Tasks
{
    public class TasksController : ITasksController
    {
        public event Action OnAllTasksFinished;

        private ILevelConfigFactory _levelConfigFactory;
        private ISaveManager _saveManager;

        private LevelConfig _currentLevelConfig;
        private List<ITask> _tasks;

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
            
            List<ITask> oldList = _currentLevelConfig._tasksToCompleteLevel._tasks;
            _tasks = new List<ITask>(oldList.Count);
            oldList.ForEach(item => _tasks.Add(item.Clone()));
        }

        public List<ITask> GetTasks() => _tasks;

        public void UpdateReceivedCards(CardType cardType)
        {
            foreach (ITask task in GetTasks())
            {
                if (task is GetCardsTask getCardsTask)
                {
                    if (getCardsTask._requiredCards.ContainsKey(cardType))
                    {
                        if (getCardsTask._requiredCards[cardType] > 0)
                            getCardsTask._requiredCards[cardType]--;

                        CheckIsTaskComplete(task);
                    }
                }
            }
        }

        private void CheckIsTaskComplete(ITask task)
        {
            bool isTaskComplete = task.IsComplete();
            if (isTaskComplete)
            {
                foreach (ITask t in GetTasks())
                {
                    if (!t.IsComplete())
                        return;
                }

                OnAllTasksFinished?.Invoke();
            }
        }
    }
}