using System;
using AYellowpaper.SerializedCollections;
using Dajjsand.Enums;

namespace Dajjsand.Models.Task
{
    [Serializable]
    public class GetCardsTask : ITask
    {
        public string _taskName;
        public SerializedDictionary<CardType, int> _requiredCards;
    }
}