using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KtoOsel.Lib
{
    public class Game
    {
        private readonly ReadOnlyCollection<GameCard> _thisStrategyCards;

        public Turn CurrentTurn { get; private set; }

        public Game(List<GameCard> thisStrategyCards, Turn currentTurn)
        {
            _thisStrategyCards = new ReadOnlyCollection<GameCard>(thisStrategyCards); 
            CurrentTurn = currentTurn;
        }

        public ReadOnlyCollection<GameCard> ThisStrategyCards
        {
            get { return _thisStrategyCards; }
        }
    }
}
