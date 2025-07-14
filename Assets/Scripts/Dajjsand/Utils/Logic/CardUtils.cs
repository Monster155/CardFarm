using UnityEngine;

namespace Dajjsand.Utils.Logic
{
    public class CardUtils
    {
        private const float CardOffsetRange = 1.2f;
        private const int CardsInFirstRound = 6;
        private const int CardsIncreaseInEachRound = 4;

        public static Vector3 CardOffset(int cardIndex)
        {
            int currentCardRound = 0;
            int cardsInRound = CardsInFirstRound;
            while (cardIndex >= cardsInRound)
            {
                currentCardRound++;
                cardIndex -= cardsInRound;
                cardsInRound = CardsInFirstRound + CardsIncreaseInEachRound * currentCardRound;
            }

            float angle = (360f / cardsInRound) * cardIndex;
            Vector3 vector = new Vector3(CardOffsetRange * (currentCardRound + 1), 0, 0);
            vector = Quaternion.AngleAxis(angle, Vector3.up) * vector;
            vector.Scale(new Vector3(0.7f, 1f, 1f));

            return vector;
        }
    }
}