using Dajjsand.Controllers.Game.Cards;

namespace Dajjsand.Controllers.Game.Factories.CardFactory
{
    public interface ICardFactory
    {
        public BaseCard GetCard();
        public bool ReleaseCard(BaseCard card);
    }
}