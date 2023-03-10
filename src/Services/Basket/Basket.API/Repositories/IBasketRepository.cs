using Basket.API.Entities;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string username);

        Task<ShoppingCart> UpdateCart(ShoppingCart cart);

        Task DeleteBasket(string username);
    }
}
