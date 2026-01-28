using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Events
{
    public record UserRegisteredEvent(string UserId);
}
