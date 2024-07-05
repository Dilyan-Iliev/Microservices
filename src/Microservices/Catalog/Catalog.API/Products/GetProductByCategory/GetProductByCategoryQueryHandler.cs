using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
    public record GetProductByCategoryResult(IEnumerable<Product> Products);


    internal class GetProductByCategoryQueryHandler
        : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        private readonly IDocumentSession _session;

        public GetProductByCategoryQueryHandler(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery request,
            CancellationToken cancellationToken)
        {
            var products = await _session.Query<Product>()
                .Where(pr => pr.Category.Contains(request.Category))
                .ToListAsync();

            return new GetProductByCategoryResult(products);
        }
    }
}
