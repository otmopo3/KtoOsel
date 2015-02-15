using System;
using System.Collections.Generic;
using System.Linq;
using KtoOsel.Lib;

namespace KtoOsel.Strategies
{
    public class WrongStrategy : IStrategy
    {
        public string Name { get { return "WrongStrategy"; } }
        public Guid Id { get { return Guid.Parse("{0E7D8DA4-C1DE-49D3-A064-05058386FD8A}"); } }

        public List<GameCard> MakeAction(Game game)
        {
            return new List<GameCard>
            {
                game.ThisStrategyCards.First()
            };
        }

        public object Control { get { return null; } }
    }
}
