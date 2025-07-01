namespace Dajjsand.Controllers.Game.Managers.Save
{
    public interface ISaveManager
    {
        public void SaveCurrentLevelIndex(int levelIndex);
        public int GetCurrentLevelIndex();
        public void SaveMaxReachedLevelIndex(int levelIndex);
        public int GetMaxReachedLevelIndex();
        public void SetStarsByLevelIndex(int levelIndex, int stars);
        public int[] GetStarsByLevelIndex();
    }
}