using Basket.API.Models;

namespace Basket.API.Data
{
    public interface IBasketRepository
    {
        //For GetBasketHandler
        Task<ShoppingCart> GetBasket(string username, CancellationToken cancellationToken = default);

        //For StoreBasketHandler
        Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default);

        //For DeleteBasketHandler
        Task<bool> DeleteBasket(string username, CancellationToken cancellationToken= default);
    }
}
