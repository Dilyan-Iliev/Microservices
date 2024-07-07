using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.CQRS;

namespace Basket.API.Basket.GetBasket
{
    public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
    public record GetBasketResult(ShoppingCart ShoppingCart);


    public class GetBasketQueryHandler
        : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        private readonly IBasketRepository _repo;

        public GetBasketQueryHandler(IBasketRepository repo)
        {
            _repo = repo;
        }

        public async Task<GetBasketResult> Handle(GetBasketQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _repo.GetBasket(request.UserName, cancellationToken);
            return new GetBasketResult(result);
        }
    }
}
