using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialManagementApp.Dto.Transaction
{
    public class CreateTransactionDto
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string Category { get; set; } = String.Empty;
    }
}