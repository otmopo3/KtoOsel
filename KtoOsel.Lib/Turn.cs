using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KtoOsel.Lib
{
    public class Turn
    {
        public readonly ReadOnlyCollection<KeyValuePair<string, List<GameCard>>> ThisTurn;

        public int Count { get; private set; }

        public Turn()
        {
            ThisTurn = new ReadOnlyCollection<KeyValuePair<string, List<GameCard>>>(new KeyValuePair<string, List<GameCard>>[0]);
            Count = 0;
        }

        private Turn(IEnumerable<KeyValuePair<string, List<GameCard>>> turn, string playerName, List<GameCard> cards)
        {
            var tmpList = new List<KeyValuePair<string, List<GameCard>>>(turn)
            {
                new KeyValuePair<string, List<GameCard>>(playerName, cards)
            };
            
            ThisTurn = new ReadOnlyCollection<KeyValuePair<string, List<GameCard>>>(tmpList);

            Count = tmpList.Count;
        }

        public Turn AddPlayerTurn(string playerName, List<GameCard> cards)
        {
            return new Turn(ThisTurn, playerName, cards);
        }
    }
}