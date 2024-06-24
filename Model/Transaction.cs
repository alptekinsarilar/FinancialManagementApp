using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialManagementApp.Model
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public string Description { get; set; } = String.Empty;
        public string Category { get; set; } = String.Empty;
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User User { get; set; } 
    }
}