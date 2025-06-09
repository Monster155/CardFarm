using UnityEngine;

namespace Dajjsand.Controllers.Game.Managers.Save
{
    public class SaveManager : ISaveManager
    {
        private int _currentLevelIndex;
        private bool _isFirstLoad;

        public SaveManager()
        {
            _currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 1);
            _isFirstLoad = PlayerPrefs.GetInt("IsFirstLoad", 1) == 1;
        }

        public void SaveCurrentLevelIndex(int levelIndex)
        {
            PlayerPrefs.SetInt("CurrentLevel", levelIndex);
            PlayerPrefs.Save();
            _currentLevelIndex = levelIndex;

            if (_isFirstLoad)
                SetFirstLoadComplete();
        }

        public void SetFirstLoadComplete()
        {
            _isFirstLoad = false;
            PlayerPrefs.SetInt("IsFirstLoad", 0);
            PlayerPrefs.Save();
        }

        public int GetCurrentLevelIndex()
        {
            return _currentLevelIndex;
        }
    }
}