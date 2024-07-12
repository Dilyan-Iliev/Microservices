using Basket.API.Data;
using Basket.API.Dtos;
using BuildingBlocks.CQRS;
using BuildingBlocks.Messaging.Events;
using FluentValidation;
using Mapster;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket
{
    public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto) : ICommand<CheckoutBasketResult>;
    public record CheckoutBasketResult(bool IsSuccess);

    public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
    {
        public CheckoutBasketCommandValidator()
        {
            RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto is required");
            RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }

    public class CheckoutBasketCommandHandler
        : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
    {
        private readonly IBasketRepository _repo;
        private readonly IPublishEndpoint _publishEndpoint; //we need this to publish an event to RabbitMQ

        public CheckoutBasketCommandHandler(IBasketRepository repo,
            IPublishEndpoint publishEndpoint)
        {
            _repo = repo;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand request,
            CancellationToken cancellationToken)
        {
            var basket = await _repo.GetBasket(request.BasketCheckoutDto.UserName, cancellationToken);

            if (basket == null)
            {
                return new CheckoutBasketResult(false);
            }

            //Prepare the event message to be sent to the message broker
            var eventMessage = request.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
            eventMessage.TotalPrice = basket.TotalPrice;

            //Publish the event message
            await _publishEndpoint.Publish(eventMessage, cancellationToken);

            //delete existing basket from db
            await _repo.DeleteBasket(request.BasketCheckoutDto.UserName, cancellationToken);

            return new CheckoutBasketResult(true);
        }
    }
}
