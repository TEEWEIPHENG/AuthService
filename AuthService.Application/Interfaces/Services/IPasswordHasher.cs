using AuthService.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Interfaces.Services
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        string EnhancedHash(string password);
        bool Verify(string password, string hash);
    }
}
