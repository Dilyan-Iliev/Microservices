using Carter;
using Mapster;
using MediatR;

namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductResponse(bool IsSuccess);

    public class DeleteProductEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
            {
                var request = new DeleteProductCommand(id);
                var result = await sender.Send(request);
                var response = result.Adapt<DeleteProductResponse>();

                return Results.Ok(response);
            })
            .WithName("DeleteProduct")
            .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Product by Id")
            .WithDescription("Delete Product by Id");
        }
    }
}
