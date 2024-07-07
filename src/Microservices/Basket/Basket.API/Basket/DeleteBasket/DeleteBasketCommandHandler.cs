using Basket.API.Data;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;
    public record DeleteBasketResult(bool IsSuccess);

    public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required");
        }
    }

    public class DeleteBasketCommandHandler
        : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        private readonly IBasketRepository _repo;

        public DeleteBasketCommandHandler(IBasketRepository repo)
        {
            _repo = repo;
        }

        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand request,
            CancellationToken cancellationToken)
        {
            var basket = await _repo.GetBasket(request.UserName, cancellationToken);
            var result = await _repo.DeleteBasket(basket.UserName, cancellationToken);
            return new DeleteBasketResult(result);
        }
    }
}
