namespace Dajjsand.Controllers.Game.Managers.Save
{
    public interface ISaveManager
    {
        public void SaveCurrentLevelIndex(int levelIndex);
        public int GetCurrentLevelIndex();
    }
}