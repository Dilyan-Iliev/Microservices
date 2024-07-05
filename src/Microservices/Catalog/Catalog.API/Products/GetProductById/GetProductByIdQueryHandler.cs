﻿using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Product Product);


    internal class GetProductByIdQueryHandler
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        private readonly IDocumentSession _session;
        private readonly ILogger<GetProductByIdQueryHandler> _logger;

        public GetProductByIdQueryHandler(IDocumentSession session, 
            ILogger<GetProductByIdQueryHandler> logger)
        {
            _session = session;
            _logger = logger;
        }

        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetProductByIdQueryHandler.Handle called with {@Request}", request);

            var product = await _session.LoadAsync<Product>(request.Id, cancellationToken);

            if (product == null)
            {
                throw new ProductNotFoundException();
            }

            return new GetProductByIdResult(product!);
        }
    }
}