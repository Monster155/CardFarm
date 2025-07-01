using Dajjsand.DataAndModel;

namespace Dajjsand.Factories.LevelConfigFactory
{
    public interface ILevelConfigFactory
    {
        public LevelConfig GetLevelConfig(int levelIndex);
        public int GetLevelsCount();
    }
}