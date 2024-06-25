using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialManagementApp.Model;

namespace FinancialManagementApp.Dto.Wallet
{
     public class CreateAccountDto
    {
        public string AccountName { get; set; } = string.Empty;
        public Currency Currency { get; set; }  // Currency type changed to enum
    }
}