using UnityEngine;

namespace Dajjsand.Controllers.Game.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelConfig 1", menuName = "Custom/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        public int LevelIndex;
    }
}