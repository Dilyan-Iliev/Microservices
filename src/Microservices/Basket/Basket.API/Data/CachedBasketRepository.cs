using Basket.API.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CachedBasketRepository
        : IBasketRepository
    {
        //this will be decorator class which adds additional functionallity to IBasketRepository runtime
        //also this class acts as a proxy and forwards the calls to the underlying basket repository

        private readonly IBasketRepository _repo;
        private readonly IDistributedCache _cache;

        public CachedBasketRepository(IBasketRepository repo, IDistributedCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<ShoppingCart> GetBasket(string username,
            CancellationToken cancellationToken = default)
        {
            //Cache-asside:

            //Try get from cache
            var cachedBasket = await _cache.GetStringAsync(username, cancellationToken);
            if (!string.IsNullOrEmpty(cachedBasket))
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
            }

            //If not in the cache - get from DB and then put in cache
            var basket = await _repo.GetBasket(username, cancellationToken);
            await _cache.SetStringAsync(username, JsonSerializer.Serialize(basket), cancellationToken);

            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket,
            CancellationToken cancellationToken = default)
        {
            await _repo.StoreBasket(basket, cancellationToken);
            await _cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);
            return basket;
        }

        public async Task<bool> DeleteBasket(string username,
            CancellationToken cancellationToken = default)
        {
            bool result = await _repo.DeleteBasket(username, cancellationToken);
            await _cache.RemoveAsync(username, cancellationToken);

            return result;
        }
    }
}
