using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstractions;

namespace Ordering.Infrastructure.Data.Interceptors
{
    public class DispatchDomainEventsInterceptor
        : SaveChangesInterceptor
    {
        private readonly IMediator _mediator;

        public DispatchDomainEventsInterceptor(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await DispatchDomainEvents(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private async Task DispatchDomainEvents(DbContext? context)
        {
            if (context == null) return;

            //Retrieve from the aggregates from the db ctx object
            //we're looking for db context entities which implement IAggregate interface
            var aggregates = context.ChangeTracker
                .Entries<IAggregate>()
                .Where(a => a.Entity.DomainEvents.Any())
                .Select(a => a.Entity);

            //retrieve domain events
            var domainEvents = aggregates
                .SelectMany(a => a.DomainEvents)
                .ToList();

            //clear domain events before dispatching them
            foreach (var agg in aggregates)
            {
                agg.ClearDomainEvents();
            }

            //dispath the events using MediatR to the consumers
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);
            }
        }
    }
}
