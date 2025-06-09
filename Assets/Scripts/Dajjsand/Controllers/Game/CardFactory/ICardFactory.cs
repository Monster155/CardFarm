using Dajjsand.Controllers.Game.Cards;

namespace Dajjsand.Controllers.Game.CardFactory
{
    public interface ICardFactory
    {
        public BaseCard GetCard();
        public bool ReleaseCard(BaseCard card);
    }
}