using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Common
{
    public abstract class AggregateRoot : Entity
    {
        private readonly List<object> _domainEvents = [];
        public IReadOnlyCollection<object> DomainEvents => _domainEvents;

        protected void AddDomainEvent(object @event)
        {
            _domainEvents.Add(@event);
        }
    }
}
