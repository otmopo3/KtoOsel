using System;
using System.Collections.Generic;
using System.Linq;
using KtoOsel.Lib;

namespace KtoOsel.Strategies
{
    public class FirstTestStrategy : IStrategy
    {
        //private readonly FirstSTrategyStatusControl _control = new FirstSTrategyStatusControl();

		private string _currentStatus;

        public FirstTestStrategy(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public Guid Id { get { return Guid.Parse("{245FFBE0-7F26-4736-AAF6-58D8D9C3CC25}"); } }

        public List<GameCard> MakeAction(Game game)
        {
            var curTurn = game.CurrentTurn;
            var myCards = game.ThisStrategyCards.ToList();

			_currentStatus = String.Join(",", myCards);

            myCards.Sort();

            var myTurnCards = new List<GameCard>();

            if (curTurn.ThisTurn.Count == 0)
            {
                var card = myCards.First();
                myCards.Remove(card);
                myTurnCards.Add(card);

                while (myCards.Count > 0 && myTurnCards.Count < 4 && card.CompareTo(myCards.First()) == 0)
                {
                    var curCard = myCards.First();
                    myCards.Remove(curCard);
                    myTurnCards.Add(curCard);
                }

                return myTurnCards;
            }

            var lastCardsInTurn = curTurn.ThisTurn.Last(cards => !(cards.Value.First() is PassCard)).Value;

            var count = lastCardsInTurn.Count;
            var maxCard = lastCardsInTurn.FirstOrDefault(card => card.ScorePoints < 15);

            if (maxCard == null)
            {
                return new List<GameCard>
                    {
                        new PassCard()
                    };
            }

            var myJokerCount = myCards.OfType<JokerCard>().Count();

            var myGreaterCard = myCards.FirstOrDefault(card => card > maxCard && !(card is AssCard));

            if (myGreaterCard == null)
                return new List<GameCard>
                    {
                        new PassCard()
                    };

            while (myGreaterCard.ScorePoints < 20)
            {
                var myGreaterCardsCount = myCards.Count(card => card.ScorePoints == myGreaterCard.ScorePoints);
                if (myGreaterCardsCount + myJokerCount >= count)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var myCard = myCards.Find(card => myGreaterCard.ScorePoints == card.ScorePoints);
                        if (myCard == null)
                            myCard = myCards.Find(card => card is JokerCard);

                        if (myCard == null)
                            throw new NullReferenceException();
                        myCards.Remove(myCard);
                        myTurnCards.Add(myCard);
                    }

                    return myTurnCards;
                }

                myGreaterCard = myCards.FirstOrDefault(card => card > myGreaterCard && !(card is AssCard));

                if (myGreaterCard == null)
                    return new List<GameCard>
                    {
                        new PassCard()
                    };
            }

            return new List<GameCard>
            {
                new PassCard()
            };
        }

		public object Control { get { return _currentStatus; } }
    }
}
