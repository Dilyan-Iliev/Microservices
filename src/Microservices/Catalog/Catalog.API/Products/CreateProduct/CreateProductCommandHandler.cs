using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description,
        string ImageFile, decimal Price) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);


    internal class CreateProductCommandHandler
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    //accepts CreateProductCommand as request type and returns CreateProductResult;
    {
        private readonly IDocumentSession _session;

        internal CreateProductCommandHandler(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<CreateProductResult> Handle(CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = new Product()
            {
                Name = request.Name,
                Category = request.Category,
                Description = request.Description,
                ImageFile = request.ImageFile,
                Price = request.Price,
            };

            _session.Store(product);
            await _session.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id);
        }
    }
}