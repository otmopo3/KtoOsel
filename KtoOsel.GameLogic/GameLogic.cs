using System;
using System.Collections.Generic;
using System.Linq;
using KtoOsel.Lib;

namespace KtoOsel.Logic
{

    public class TurnResult
    {
        public bool EndGame;
        public string TurnSummary;
    }

    public class GameLogic
    {
        private readonly List<IStrategy> _strategies;
        private readonly List<List<GameCard>> _strategysCards = new List<List<GameCard>>();
        private readonly Random _random;
        private readonly List<GameCard> _cardPack;
        private int _curStrategyNum;
        private int _startStrategyNum;
        Turn _curTurn = new Turn();
        private readonly List<Turn> _turns = new List<Turn>();
        private readonly RulesChecker _rulesChecker = new RulesChecker();

        public GameLogic(List<IStrategy> strategies, int seed = 0)
        {
            _strategies = strategies;
            _random = new Random(seed);
            _cardPack = GetCardPack();
            FillPlayersCards();

            _curStrategyNum = FindFirstStrategyNum(_strategysCards);
            _startStrategyNum = _curStrategyNum;
        }

        public TurnResult Next()
        {
            var thisTurnResult = new TurnResult();
            var curStrategy = _strategies[_curStrategyNum];

            var thisPlayerCards = new List<GameCard>(_strategysCards[_curStrategyNum]);

            var game = new Game(thisPlayerCards, _curTurn);

            var cards = curStrategy.MakeAction(game);

            thisTurnResult.TurnSummary = curStrategy.Name + "  " + String.Join(", ", cards);

            _rulesChecker.CheckTurnForRules(game, cards);

            _strategysCards[_curStrategyNum].RemoveAll(cards.Contains);

            _curTurn = _curTurn.AddPlayerTurn(curStrategy.Name, cards);

            if (_curTurn.Count == _strategies.Count)
            {
                _turns.Add(_curTurn);
                _curTurn = new Turn();
                thisTurnResult.TurnSummary += "\n Next round ";
            }

            if (cards.All(c => c is PassCard))
            {
                GoToNextStrategy();
                return thisTurnResult;
            }

            if (_strategysCards[_curStrategyNum].Count == 0)
            {
                thisTurnResult.EndGame = true;
                return thisTurnResult;
            }

            GoToNextStrategy();
            return thisTurnResult;
        }
        
        private void GoToNextStrategy()
        {
            if (_curTurn.Count == 0 && _turns.Count > 0)
            {
                var lastTurn = _turns.Last();
                var winnerKvp = lastTurn.ThisTurn.Last(cards => !cards.Value.Any(c => c is PassCard));
                var winnerPos = lastTurn.ThisTurn.IndexOf(winnerKvp);
                _startStrategyNum += winnerPos;
                _startStrategyNum = _startStrategyNum % _strategies.Count;
                _curStrategyNum = _startStrategyNum;
                return;
            }

            _curStrategyNum++;
            if (_curStrategyNum >= _strategies.Count)
                _curStrategyNum = 0;
        }

        private int FindFirstStrategyNum(List<List<GameCard>> strategysCards)
        {
            int i = 0;
            foreach (var strategyCards in strategysCards)
            {
                if (strategyCards.OfType<AssCard>().Any())
                    return i;

                i++;
            }
            throw new NoAssInPlayersCardsException();
        }

        private static List<GameCard> GetCardPack()
        {
            var cardPack = new List<GameCard>();
            for (int i = 1; i <= 13; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    cardPack.Add(new NumberCard(i));
                }
            }

            for (int j = 1; j <= 5; j++)
            {
                cardPack.Add(new JokerCard());
            }

            cardPack.Add(new AssCard());

            return cardPack;
        }

        private void FillPlayersCards()
        {
            // ReSharper disable once UnusedVariable
            foreach (var strategy in _strategies)
            {
                var strategyCards = GetRandomCards(13);
                _strategysCards.Add(strategyCards);
            }

            var assCard = FindAssInPack();
            if (assCard != null)
            {
                var firstStrategyCards = _strategysCards.First();
                firstStrategyCards[0] = assCard;
            }
        }

        private GameCard FindAssInPack()
        {
            foreach (var gameCard in _cardPack)
            {
                if (gameCard is AssCard)
                {
                    _cardPack.Remove(gameCard);
                    return gameCard;
                }
            }
            return null;
        }

        private List<GameCard> GetRandomCards(int num)
        {
            var strategyCards = new List<GameCard>();
            for (int i = 0; i < num; i++)
            {
                var rndNum = _random.Next(_cardPack.Count);
                var card = _cardPack[rndNum];
                _cardPack.RemoveAt(rndNum);
                strategyCards.Add(card);
            }
            return strategyCards;
        }
    }

    internal class DifferentCardsException : Exception
    {
    }

    internal class MoreThan3CardException : Exception
    {
    }

    public class WrongCardReturnedException : Exception
    {
    }

    internal class NoAssInPlayersCardsException : Exception
    {
    }
}
