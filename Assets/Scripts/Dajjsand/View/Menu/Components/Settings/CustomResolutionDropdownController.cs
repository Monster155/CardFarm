using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dajjsand.View.Menu.Components.Settings
{
    public class CustomResolutionDropdownController : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _resolutionsDropdown;

        private Resolution[] _resolutions;

        private void Start()
        {
            _resolutions = Screen.resolutions;
            _resolutionsDropdown.ClearOptions();
            List<string> options = new List<string>();
            int currentResolutionIndex = 0;
            for (int i = 0; i < _resolutions.Length; i++)
            {
                options.Add(_resolutions[i].width + "x" + _resolutions[i].height);
                if (_resolutions[i].width == Screen.currentResolution.width &&
                    _resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }
            _resolutionsDropdown.AddOptions(options);
            _resolutionsDropdown.value = currentResolutionIndex;
            _resolutionsDropdown.RefreshShownValue();

            _resolutionsDropdown.onValueChanged.AddListener(ResolutionsDropdown_OnValueChanged);
        }

        private void ResolutionsDropdown_OnValueChanged(int resolutionIndex)
        {
            Resolution resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
    }
}