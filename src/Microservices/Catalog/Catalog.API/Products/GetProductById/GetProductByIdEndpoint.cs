using Carter;
using Catalog.API.Models;
using Mapster;
using MediatR;

namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdResponse(Product Product);

    public class GetProductByIdEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
            {
                var request = await sender.Send(new GetProductByIdQuery(id));
                var response = request.Adapt<GetProductByIdResponse>();

                return Results.Ok(response);
            })
            .WithName("GetProductById")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product By Id")
            .WithDescription("Get Product By Id");
        }
    }
}
