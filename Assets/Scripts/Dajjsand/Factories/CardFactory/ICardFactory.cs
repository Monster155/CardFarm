using System.Collections.Generic;
using Dajjsand.Controllers.GameLoading;
using Dajjsand.Enums;
using Dajjsand.ScriptableObjects;
using Dajjsand.View.Game.Cards;
using UnityEngine;

namespace Dajjsand.Factories.CardFactory
{
    public interface ICardFactory : ILoadable
    {
        BaseCard GetCard(CardType cardType, Vector3 pos);
        bool ReleaseCard(BaseCard card);
        List<BaseCard> GetPacks(List<CardPackData> packs);
        BaseCard GetPack(CardPackData packData);
    }
}