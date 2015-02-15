using System.Collections.Generic;
using System.Linq;
using KtoOsel.Lib;

namespace KtoOsel.Logic
{
    class RulesChecker
    {
        public void CheckTurnForRules(Game game, List<GameCard> cards)
        {
            if (cards.All(c => c is PassCard))
                return;

            if (cards.Count > 3)
            {
                throw new MoreThan3CardException();
            }

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var gameCard in cards)
            {
                if (!game.ThisStrategyCards.Contains(gameCard))
                {
                    throw new WrongCardReturnedException();
                }
            }

            if (game.CurrentTurn.Count == 0)
            {
                if (!AreCardsAllTheSame(cards))
                {
                    throw new DifferentCardsException();
                }
            }

            var areAllCardsJokers = cards.All(c => c is JokerCard);
            if (areAllCardsJokers)
            {
                // var areAllPreviousCardJokeers = 
            }


            foreach (var gameCard in cards)
            {

            }
        }

        private static bool AreCardsAllTheSame(List<GameCard> cards)
        {
            return true;
        }
    }
}
