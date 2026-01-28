using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Common
{
    public abstract class Entity
    {
        public string Id { get; protected set; } = Guid.NewGuid().ToString();
    }
}
