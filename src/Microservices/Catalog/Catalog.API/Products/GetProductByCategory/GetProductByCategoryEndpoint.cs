using Carter;
using Catalog.API.Models;
using Mapster;
using MediatR;

namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryResponse(IEnumerable<Product> Products);


    public class GetProductByCategoryEndpoint
        : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
            {
                var request = await sender.Send(new GetProductByCategoryQuery(category));
                var response = request.Adapt<GetProductByCategoryResponse>();

                return Results.Ok(response);
            })
            .WithName("GetProductsByCategory")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products By Category")
            .WithDescription("Get Products By Category");
        }
    }
}
