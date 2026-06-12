using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Dajjsand.View.Menu.Components.Settings
{
    public class CustomAudioVolumeController : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private string _audioMixerGroupName;
        [SerializeField] private string _playerPrefsKey;

        private void Start()
        {
            // setup listener
            _slider.onValueChanged.AddListener(Slider_OnValueChanged);
            
            // load
            _slider.SetValueWithoutNotify(PlayerPrefs.GetFloat(_playerPrefsKey, 80));
        }
        
        private void SetVolume(float volume)
        {
            _audioMixer.SetFloat(_audioMixerGroupName, volume);
            PlayerPrefs.SetFloat(_playerPrefsKey, volume);
        }

        private void Slider_OnValueChanged(float value) => SetVolume(value);
    }
}