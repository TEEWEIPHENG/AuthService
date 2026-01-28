using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Models
{
    public record UserProfileDto(string UserId,string Username,string Email,string Role);
}
