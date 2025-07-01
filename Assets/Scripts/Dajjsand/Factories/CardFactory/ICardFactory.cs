using Dajjsand.Controllers.GameLoading;
using Dajjsand.Enums;
using Dajjsand.View.Game.Cards;

namespace Dajjsand.Factories.CardFactory
{
    public interface ICardFactory : ILoadable
    {
        public BaseCard GetCard(CraftIngredient ingredient);
        public bool ReleaseCard(BaseCard card);
    }
}