using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialManagementApp.Dto.Transaction;
using FinancialManagementApp.Model;

namespace FinancialManagementApp.Dto.Wallet
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public Currency Currency { get; set; }
    }
}