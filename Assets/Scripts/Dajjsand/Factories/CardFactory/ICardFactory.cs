using System.Collections.Generic;
using Dajjsand.Controllers.GameLoading;
using Dajjsand.Enums;
using Dajjsand.View.Game.Cards;
using UnityEngine;

namespace Dajjsand.Factories.CardFactory
{
    public interface ICardFactory : ILoadable
    {
        public BaseCard GetCard(CardType ingredientType, Vector3 pos);
        public bool ReleaseCard(BaseCard card);
        BaseCard GetStarterPack(Dictionary<CardType, int> ingredients);
    }
}