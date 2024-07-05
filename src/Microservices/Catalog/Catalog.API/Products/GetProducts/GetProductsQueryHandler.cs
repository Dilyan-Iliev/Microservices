using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.GetProducts
{
    public record GetProductsQuery() : IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Product> Products);


    internal class GetProductsQueryHandler
        : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        private readonly IDocumentSession _session;
        private readonly ILogger<GetProductsQueryHandler> _logger;

        public GetProductsQueryHandler(IDocumentSession session,
            ILogger<GetProductsQueryHandler> logger)
        {
            _session = session;
            _logger = logger;
        }

        public async Task<GetProductsResult> Handle(GetProductsQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetProductsQueryHandler.Handle called with {@Request}", request);

            var products = await _session.Query<Product>().ToListAsync(cancellationToken);

            return new GetProductsResult(products);
        }
    }
}
