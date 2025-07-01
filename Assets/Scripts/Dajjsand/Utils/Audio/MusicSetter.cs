using UnityEngine;

namespace Dajjsand.Utils.Audio
{
    public class MusicSetter : MonoBehaviour
    {
        [SerializeField] private AudioClip _themeMusic;

        private void Start()
        {
            AudioController.Instance.SetThemeMusic(_themeMusic);
        }
    }
}