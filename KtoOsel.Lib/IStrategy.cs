using System;
using System.Collections.Generic;

namespace KtoOsel.Lib
{
    public interface IStrategy
    {
        string Name { get; }

        Guid Id { get; }

        List<GameCard> MakeAction(Game game);

        object Control { get; }
    }
}
