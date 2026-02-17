using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.API.Models.Requests
{
    public class ResetPasswordRequest
    {
        public string NewPassword { get; set; }
        public string MFAToken { get; set; }
    }
}