using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Dajjsand.View.Game.PauseMenu
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseMenuView;
        [SerializeField] private Button _transparentBGButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _toMenuButton;

        private void Start()
        {
            _pauseMenuView.gameObject.SetActive(false);

            _transparentBGButton.onClick.AddListener(TransparentBGButton_OnClick);
            _continueButton.onClick.AddListener(ContinueButton_OnClick);
            _restartButton.onClick.AddListener(RestartButton_OnClick);
            _toMenuButton.onClick.AddListener(ToMenuButton_OnClick);
        }

        private void Update()
        {
            if (Input.GetKeyDown("escape"))
            {
                _pauseMenuView.gameObject.SetActive(!_pauseMenuView.gameObject.activeSelf);
            }
        }

        private void TransparentBGButton_OnClick() => 
            _pauseMenuView.gameObject.SetActive(false);
        private void ContinueButton_OnClick() => 
            _pauseMenuView.gameObject.SetActive(false);

        private void RestartButton_OnClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void ToMenuButton_OnClick()
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}