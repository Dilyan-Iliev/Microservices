using BuildingBlocks.CQRS;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description,
        string ImageFile, decimal Price) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator
        : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(r => r.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(r => r.Category).NotEmpty().WithMessage("Category is required");
            RuleFor(r => r.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(r => r.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }


    internal class CreateProductCommandHandler
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    //accepts CreateProductCommand as request type and returns CreateProductResult;
    {
        private readonly IDocumentSession _session;

        public CreateProductCommandHandler(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<CreateProductResult> Handle(CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            //This is not very good way to check for validation errors since this will be repeated in every handler
            //We should move this into pipeline in BuildingBlocks project
            //var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            //var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();

            //if (errors.Any())
            //{
            //    throw new ValidationException(errors.FirstOrDefault());
            //}

            //The validation from the pipeline will be used instead upon request


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