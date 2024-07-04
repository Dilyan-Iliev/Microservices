using MediatR;

namespace BuildingBlocks.CQRS
{
    public interface ICommand<TResponse>
        : IRequest<TResponse> 
        where TResponse : notnull
    {
    }

    public interface ICommand
        : ICommand<Unit>
    {
    }
}
