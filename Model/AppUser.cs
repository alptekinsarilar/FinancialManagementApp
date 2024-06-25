using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FinancialManagementApp.Model
{
    public class AppUser : IdentityUser
    {
        public ICollection<Account> Accounts { get; set; }
    }
}