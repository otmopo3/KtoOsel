using System;
using System.Globalization;

namespace KtoOsel.Lib
{
    public abstract class GameCard : IComparable<GameCard>
    {
        public abstract int ScorePoints { get; }

        public int CompareTo(GameCard other)
        {
            return ScorePoints.CompareTo(other.ScorePoints);
        }

        public static bool operator >(GameCard thisCard, GameCard otherCard)
        {
            return thisCard.CompareTo(otherCard) > 0;
        }

        public static bool operator <(GameCard thisCard, GameCard otherCard)
        {
            return thisCard.CompareTo(otherCard) < 0;
        }
    }

    public class NumberCard : GameCard
    {
        private readonly int _number;

        public NumberCard(int number)
        {
            _number = number;
        }

        public int Number { get { return _number; } }
        public override int ScorePoints { get { return _number; } }

        public override string ToString()
        {
            return _number.ToString(CultureInfo.InvariantCulture);
        }
    }

    public class JokerCard : GameCard
    {
        public override int ScorePoints { get { return 14; } }

        public override string ToString()
        {
            return "Joker";
        }
    }

    public class AssCard : GameCard
    {
        public override int ScorePoints { get { return 20; } }

        public override string ToString()
        {
            return "Ass";
        }
    }

    public class PassCard : GameCard
    {
        public override int ScorePoints { get { return 0; } }

        public override string ToString()
        {
            return "PASS";
        }
    }
}
