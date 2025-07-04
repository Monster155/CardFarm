using System;
using System.Text;
using Dajjsand.Controllers.Tasks;
using Dajjsand.Factories.LevelConfigFactory;
using Dajjsand.Models.Task;
using TMPro;
using UnityEngine;
using Zenject;

namespace Dajjsand.View.Game.Tasks
{
    public class TasksView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tasksText;

        private ITasksController _tasksController;
        private ILevelConfigFactory _levelConfigFactory;

        [Inject]
        private void Construct(ITasksController tasksController, ILevelConfigFactory levelConfigFactory)
        {
            _tasksController = tasksController;
            _levelConfigFactory = levelConfigFactory;
            _levelConfigFactory.OnLoadComplete += LevelConfigFactory_OnLoadComplete;
        }

        private void OnDestroy()
        {
            _levelConfigFactory.OnLoadComplete -= LevelConfigFactory_OnLoadComplete;
        }

        private void UpdateText()
        {
            StringBuilder sb = new StringBuilder();
            foreach (ITask task in _tasksController.GetTasks())
                sb.AppendLine(task.GetTaskText());

            _tasksText.text = sb.ToString();
        }

        private void LevelConfigFactory_OnLoadComplete()
        {
            foreach (ITask task in _tasksController.GetTasks())
                task.OnTaskUpdated += Task_OnTaskUpdated;

            UpdateText();
        }

        private void Task_OnTaskUpdated(ITask task)
        {
            UpdateText();
        }
    }
}