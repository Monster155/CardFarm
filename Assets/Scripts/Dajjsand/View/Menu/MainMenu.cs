using UnityEngine;
using UnityEngine.UI;

namespace Dajjsand.View.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;
        [Space]
        [SerializeField] private BaseScreen _settingsScreen;
        [SerializeField] private BaseScreen _selectLevelScreen;

        private void Start()
        {
            _playButton.onClick.AddListener(PlayButton_OnClick);
            _settingsButton.onClick.AddListener(SettingsButton_OnClick);
            _exitButton.onClick.AddListener(ExitButton_OnClick);
        }

        private void PlayButton_OnClick() => _selectLevelScreen.Show();
        private void SettingsButton_OnClick() => _settingsScreen.Show();
        private void ExitButton_OnClick() => Application.Quit();
    }
}