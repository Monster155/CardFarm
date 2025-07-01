using UnityEngine;

namespace Dajjsand.Managers.Save
{
    public class SaveManager : ISaveManager
    {
        private int _currentLevelIndex;
        private int _maxReachedLevelIndex;
        private int[] _starsByLevelIndex;

        public SaveManager()
        {
            _currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 0);
            _maxReachedLevelIndex = PlayerPrefs.GetInt("MaxReachedLevelIndex", 0);
            
            _starsByLevelIndex = new int[_maxReachedLevelIndex + 1];
            for (int levelIndex = 0; levelIndex < _maxReachedLevelIndex; levelIndex++)
                _starsByLevelIndex[levelIndex] = PlayerPrefs.GetInt($"StarsByLevelIndex_{levelIndex}", 0);
        }

        public void SaveCurrentLevelIndex(int levelIndex)
        {
            PlayerPrefs.SetInt("CurrentLevelIndex", levelIndex);
            PlayerPrefs.Save();
            _currentLevelIndex = levelIndex;
        }

        public int GetCurrentLevelIndex() => _currentLevelIndex;
        
        public void SaveMaxReachedLevelIndex(int levelIndex)
        {
            PlayerPrefs.SetInt("MaxReachedLevelIndex", levelIndex);
            PlayerPrefs.Save();
            _maxReachedLevelIndex = levelIndex;
        }
        
        public int GetMaxReachedLevelIndex() => _maxReachedLevelIndex;
        
        public void SetStarsByLevelIndex(int levelIndex, int stars)
        {
            PlayerPrefs.SetInt($"StarsByLevelIndex_{levelIndex}", levelIndex);
            PlayerPrefs.Save();
        }
        
        public int[] GetStarsByLevelIndex() => _starsByLevelIndex;
    }
}