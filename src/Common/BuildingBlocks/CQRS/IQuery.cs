using MediatR;

namespace BuildingBlocks.CQRS
{
    public interface IQuery<TResponse>
        : IRequest<TResponse>
    {
    }

    public interface IQuery
        : IQuery<Unit>
    {
    }
}
