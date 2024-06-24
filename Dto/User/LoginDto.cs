using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialManagementApp.Dto.User
{
    public class LoginDto
    {
        public string Email { set; get; } = String.Empty;
        public string Password { set; get; } = String.Empty;
    }
}