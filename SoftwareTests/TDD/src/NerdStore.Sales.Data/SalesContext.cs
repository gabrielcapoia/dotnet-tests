﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using NerdStore.Core.Data;
using NerdStore.Core.DomainObjects;
using System.Threading.Tasks;
using System.Linq;

namespace NerdStore.Sales.Data
{
    public class SalesContext : DbContext, IUnitOfWork
    {
        private readonly IMediator mediator;

        public SalesContext(DbContextOptions<SalesContext> options, IMediator mediator)
            : base(options)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Commit()
        {
            var success = await base.SaveChangesAsync() > 0;
            if (success) await mediator.PublishEvents(this);

            return success;
        }
    }

    public static class MediatorExtension
    {
        public static async Task PublishEvents(this IMediator mediator, SalesContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notifications != null && x.Entity.Notifications.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notifications)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) => {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
