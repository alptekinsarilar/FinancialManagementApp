using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialManagementApp.Dto.Account;

namespace FinancialManagementApp.Dto.Transaction
{
    public class ResponseTransactionDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedAt { get; set; } 
 
    }

}