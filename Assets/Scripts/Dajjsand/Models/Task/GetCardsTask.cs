using System;
using System.Linq;
using System.Text;
using AYellowpaper.SerializedCollections;
using Dajjsand.Enums;

namespace Dajjsand.Models.Task
{
    [Serializable]
    public class GetCardsTask : ITask
    {
        public event Action<ITask> OnTaskUpdated;

        public string _taskName;
        public SerializedDictionary<CardType, int> _requiredCards;

        public string GetTaskText()
        {
            StringBuilder sb = new StringBuilder("Receive cards:\n");

            foreach ((CardType type, int count) in _requiredCards)
                sb.Append($"\t{type}: {count}\n");
            
            return sb.ToString();
        }
        
        public bool IsComplete()
        {
            OnTaskUpdated?.Invoke(this);
            return _requiredCards.Values.All(count => count == 0);
        }
        public ITask Clone()
        {
            return new GetCardsTask()
            {
                _taskName = _taskName,
                _requiredCards = new SerializedDictionary<CardType, int>(_requiredCards),
            };
        }
    }
}