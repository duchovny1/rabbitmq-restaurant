using Prices.Domain.ViewModels.Request;
using System.Threading.Tasks;

namespace Prices.Business
{
    public interface IChangesHistoryService
    {
        Task AddToHistory(long productId, SetProductPriceRequest request);
    }
}
