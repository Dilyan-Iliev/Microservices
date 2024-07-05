using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, string Description, string ImageFile,
        decimal Price, List<string> Category) : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("Product ID is required");

            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");

            RuleFor(command => command.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

    internal class UpdateProductCommandHandler
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        private readonly IDocumentSession _session;

        public UpdateProductCommandHandler(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<UpdateProductResult> Handle(UpdateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _session.LoadAsync<Product>(request.Id);

            if (product == null)
            {
                throw new ProductNotFoundException(request.Id);
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
