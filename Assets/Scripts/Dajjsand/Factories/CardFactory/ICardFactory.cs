using Dajjsand.DataAndModel.Cards;

namespace Dajjsand.Factories.CardFactory
{
    public interface ICardFactory
    {
        public BaseCard GetCard();
        public bool ReleaseCard(BaseCard card);
    }
}