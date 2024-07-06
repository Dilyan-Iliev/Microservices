using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;
using Marten.Pagination;

namespace Catalog.API.Products.GetProducts
{
    public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Product> Products);


    internal class GetProductsQueryHandler
        : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        private readonly IDocumentSession _session;

        public GetProductsQueryHandler(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<GetProductsResult> Handle(GetProductsQuery request,
            CancellationToken cancellationToken)
        {
            var products = await _session.Query<Product>()
                .ToPagedListAsync(request.PageNumber ?? 1, request.PageSize ?? 10, cancellationToken);

            return new GetProductsResult(products);
        }
    }
}
