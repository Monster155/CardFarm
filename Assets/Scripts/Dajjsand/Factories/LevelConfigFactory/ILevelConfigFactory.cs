using Dajjsand.Controllers.GameLoading;
using Dajjsand.ScriptableObjects;

namespace Dajjsand.Factories.LevelConfigFactory
{
    public interface ILevelConfigFactory : ILoadable
    {
        public LevelConfig GetLevelConfig(int levelIndex);
        public int GetLevelsCount();
    }
}