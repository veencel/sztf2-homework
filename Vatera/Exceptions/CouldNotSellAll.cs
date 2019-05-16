using System;
using Vatera.Models;

namespace Vatera.Exceptions
{
    class CouldNotSellAll: Exception
    {
        public AuctionResult Result;

        public CouldNotSellAll(AuctionResult result)
        {
            Result = result;
        }
    }
}
