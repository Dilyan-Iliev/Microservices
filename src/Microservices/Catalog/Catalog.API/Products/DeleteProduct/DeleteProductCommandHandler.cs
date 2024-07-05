using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);

    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");
        }
    }

    internal class DeleteProductCommandHandler
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        private readonly IDocumentSession _session;
        private readonly ILogger<DeleteProductCommandHandler> _logger;

        public DeleteProductCommandHandler(IDocumentSession session, 
            ILogger<DeleteProductCommandHandler> logger)
        {
            _session = session;
            _logger = logger;
        }

        public async Task<DeleteProductResult> Handle(DeleteProductCommand request, 
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("DeleteProductCommandHandler.Handle called with {@Request}", request);

            var product = await _session.LoadAsync<Product>(request.Id);

            if (product == null)
            {
                throw new ProductNotFoundException();
            }

            _session.Delete<Product>(request.Id);
            await _session.SaveChangesAsync(cancellationToken);

            return new DeleteProductResult(true);
        }
    }
}
