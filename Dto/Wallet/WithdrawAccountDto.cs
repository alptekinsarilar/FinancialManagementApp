using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialManagementApp.Dto.Wallet
{
    public class WithdrawAccountDto
    {
        [Required]
        public int AccountId { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Withdrawal amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}