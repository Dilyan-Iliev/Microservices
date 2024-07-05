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
        private readonly ILogger<GetProductByCategoryQueryHandler> _logger;

        public GetProductByCategoryQueryHandler(IDocumentSession session, 
            ILogger<GetProductByCategoryQueryHandler> logger)
        {
            _session = session;
            _logger = logger;
        }

        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetProductByCategoryQueryHandler.Handle called with {@Request}", request);

            var products = await _session.Query<Product>()
                .Where(pr => pr.Category.Contains(request.Category))
                .ToListAsync();

            return new GetProductByCategoryResult(products);
        }
    }
}
