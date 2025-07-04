using Dajjsand.Controllers.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Dajjsand.View.Game.LevelFinished
{
    public class LevelFinishedView : MonoBehaviour
    {
        [SerializeField] private GameObject _view;
        [SerializeField] private Button _toMenuButton;

        private ITasksController _tasksController;

        [Inject]
        private void Construct(ITasksController tasksController)
        {
            _tasksController = tasksController;
            _tasksController.OnAllTasksFinished += TasksController_OnAllTasksFinished;
        }

        private void Start()
        {
            _toMenuButton.onClick.AddListener(ToMenuButton_OnClick);
            _view.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _tasksController.OnAllTasksFinished -= TasksController_OnAllTasksFinished;
            _toMenuButton.onClick.RemoveListener(ToMenuButton_OnClick);
        }

        private void TasksController_OnAllTasksFinished()
        {
            _view.gameObject.SetActive(true);
        }

        private void ToMenuButton_OnClick()
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}