using Dajjsand.Controllers.Game.ScriptableObjects;

namespace Dajjsand.Controllers.Game.Factories.LevelConfigFactory
{
    public interface ILevelConfigFactory
    {
        public LevelConfig GetLevelConfig(int levelIndex);
        public int GetLevelsCount();
    }
}