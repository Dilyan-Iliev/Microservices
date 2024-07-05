using MediatR;

namespace BuildingBlocks.CQRS
{
    public interface ICommand<TResponse>
        : IRequest<TResponse> 
    {
    }

    public interface ICommand
        : ICommand<Unit>
    {
    }
}
