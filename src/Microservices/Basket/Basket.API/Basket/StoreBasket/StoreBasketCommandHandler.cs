using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Username is required");
        }
    }

    public class StoreBasketCommandHandler
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        private readonly IBasketRepository _repo;

        public StoreBasketCommandHandler(IBasketRepository repo)
        {
            _repo = repo;
        }

        public async Task<StoreBasketResult> Handle(StoreBasketCommand request,
            CancellationToken cancellationToken)
        {
            await _repo.StoreBasket(request.Cart, cancellationToken);

            return new StoreBasketResult(request.Cart.UserName);
        }
    }
}
