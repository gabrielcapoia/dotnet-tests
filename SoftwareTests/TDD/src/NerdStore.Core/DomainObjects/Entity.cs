using NerdStore.Core.Messages;
using System;
using System.Collections.Generic;

namespace NerdStore.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        private List<Event> notifications;
        public IReadOnlyCollection<Event> Notifications => notifications?.AsReadOnly();

        public Entity()
        {
            Id = Guid.NewGuid();
        }

        public void InsertEvent(Event eventItem)
        {
            notifications ??= new List<Event>();
            notifications.Add(eventItem);
        }

        public void RemoveEvent(Event eventItem)
        {
            notifications?.Remove(eventItem);
        }

        public void ClearEvents()
        {
            notifications?.Clear();
        }
    }
}
