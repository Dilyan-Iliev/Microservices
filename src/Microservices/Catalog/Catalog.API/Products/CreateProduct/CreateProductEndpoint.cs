using Carter;
using Mapster;
using MediatR;

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductRequest(string Name, List<string> Category, string Description,
        string ImageFile, decimal Price);
    public record CreateProductResponse(Guid Id);


    public class CreateProductEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products",
                async (CreateProductRequest request, ISender sender) =>
            {
                //convert the Request into Command
                //or we can accept directly the command as param
                var command = request.Adapt<CreateProductCommand>();
                var result = await sender.Send(command);

                //or we can accept directly return the result from the CommandHandler
                var response = result.Adapt<CreateProductResponse>();

                return Results.Created($"products/{response.Id}", response);
            })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product");
        }
    }
}
