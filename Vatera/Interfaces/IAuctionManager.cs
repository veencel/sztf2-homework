using Vatera.Models;

namespace Vatera.Interfaces
{
    public interface IAuctionManager
    {
        AuctionResult FinishAuction(Product product);
    }
}
