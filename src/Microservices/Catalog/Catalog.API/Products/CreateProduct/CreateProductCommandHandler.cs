using BuildingBlocks.CQRS;

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description,
        string ImageFile, decimal Price) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);


    internal class CreateProductCommandHandler
        : ICommandHandler<CreateProductCommand, CreateProductResult>
        //accepts CreateProductCommand as request type and returns CreateProductResult;
    {
        public Task<CreateProductResult> Handle(CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
