using UnityEngine;

namespace Dajjsand.Managers.Save
{
    public class SaveManager : ISaveManager
    {
        private const string CurrentLevelIndex = "CurrentLevelIndex";
        private const string MaxReachedLevelIndex = "MaxReachedLevelIndex";
        private string StarsByLevelIndexText(int levelIndex) => $"StarsByLevelIndex_{levelIndex}";

        private int _currentLevelIndex;
        private int _maxReachedLevelIndex;
        private int[] _starsByLevelIndex;

        public SaveManager()
        {
            _currentLevelIndex = PlayerPrefs.GetInt(CurrentLevelIndex, 0);
            _maxReachedLevelIndex = PlayerPrefs.GetInt(MaxReachedLevelIndex, 0);

            _starsByLevelIndex = new int[_maxReachedLevelIndex + 1];
            for (int levelIndex = 0; levelIndex < _starsByLevelIndex.Length; levelIndex++)
            {
                _starsByLevelIndex[levelIndex] = PlayerPrefs.GetInt(StarsByLevelIndexText(levelIndex), 0);
                Debug.LogError($"Stars: {levelIndex}={_starsByLevelIndex[levelIndex]}");
            }
        }

        public void SaveCurrentLevelIndex(int levelIndex)
        {
            PlayerPrefs.SetInt(CurrentLevelIndex, levelIndex);
            PlayerPrefs.Save();
            _currentLevelIndex = levelIndex;
            Debug.LogError($"Save Current: {levelIndex}");
        }

        public int GetCurrentLevelIndex() => _currentLevelIndex;

        public void SaveMaxReachedLevelIndex(int levelIndex)
        {
            PlayerPrefs.SetInt(MaxReachedLevelIndex, levelIndex);
            PlayerPrefs.Save();
            _maxReachedLevelIndex = levelIndex;
            Debug.LogError($"Save Max: {levelIndex}");
        }

        public int GetMaxReachedLevelIndex() => _maxReachedLevelIndex;

        public void SetStarsByLevelIndex(int levelIndex, int stars)
        {
            PlayerPrefs.SetInt(StarsByLevelIndexText(levelIndex), stars);
            PlayerPrefs.Save();
            _starsByLevelIndex[levelIndex] = stars;
            Debug.LogError($"Save Stars: {levelIndex} = {stars}");
        }

        public int GetStarsByLevelIndex(int levelIndex) => _starsByLevelIndex[levelIndex];
        public int[] GetAllStarsByLevel() => _starsByLevelIndex;
    }
}