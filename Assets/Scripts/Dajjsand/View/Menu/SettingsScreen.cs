using System;
using System.Collections.Generic;
using Dajjsand.View.CustomComponents;
using Dajjsand.View.Menu.Components.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dajjsand.View.Menu
{
    public class SettingsScreen : BaseScreen
    {
        [Header("Buttons")]
        [SerializeField] private Button _backButton;
        [Header("Volume Control")]
        [SerializeField] private CustomAudioVolumeController _musicVolumeController;
        [SerializeField] private CustomAudioVolumeController _soundVolumeController;
        [Header("Screen")]
        [SerializeField] private Toggle _fullscreenToggle;


        private void Start()
        {
            // buttons
            _backButton.onClick.AddListener(BackButton_OnClick);
            
            // screen
            _fullscreenToggle.onValueChanged.AddListener(FullscreenToggle_OnValueChanged);
            _fullscreenToggle.isOn = Screen.fullScreen;
        }

        private void OnDisable()
        {
            // saving all changed settings on closing settings window
            PlayerPrefs.Save();
        }

        private void BackButton_OnClick() => Hide();
        private void FullscreenToggle_OnValueChanged(bool isEnabled) => Screen.fullScreen = isEnabled;
    }
}