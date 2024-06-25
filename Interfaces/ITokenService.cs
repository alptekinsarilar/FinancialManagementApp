using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialManagementApp.Model;

namespace FinancialManagementApp.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}