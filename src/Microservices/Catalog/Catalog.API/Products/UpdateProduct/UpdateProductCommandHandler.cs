using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, string Description, string ImageFile,
        decimal Price, List<string> Category) : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);


    internal class UpdateProductCommandHandler
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        private readonly IDocumentSession _session;
        private readonly ILogger<UpdateProductCommandHandler> _logger;

        public UpdateProductCommandHandler(IDocumentSession session,
            ILogger<UpdateProductCommandHandler> logger)
        {
            _session = session;
            _logger = logger;
        }

        public async Task<UpdateProductResult> Handle(UpdateProductCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("UpdateProducthandler.Handle called with {@Request}", request);

            var product = await _session.LoadAsync<Product>(request.Id);

            if (product == null)
            {
                throw new ProductNotFoundException();
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Category = request.Category;
            product.ImageFile = request.ImageFile;
            product.Price = request.Price;

            _session.Update(product);
            await _session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
