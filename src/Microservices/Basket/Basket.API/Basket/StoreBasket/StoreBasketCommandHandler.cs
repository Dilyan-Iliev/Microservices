using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.CQRS;
using Discount.Grpc;
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
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProto;

        public StoreBasketCommandHandler(IBasketRepository repo,
            DiscountProtoService.DiscountProtoServiceClient proto)
        {
            _repo = repo;
            _discountProto = proto;
        }

        public async Task<StoreBasketResult> Handle(StoreBasketCommand request,
            CancellationToken cancellationToken)
        {
            //communicate with Discount.gRPC and calculate latest price of products into shopping cart
            foreach (var item in request.Cart.Items)
            {
                var coupon = await _discountProto
                    .GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
                item.Price -= coupon.Amount;
            }

            await _repo.StoreBasket(request.Cart, cancellationToken);

            return new StoreBasketResult(request.Cart.UserName);
        }
    }
}
