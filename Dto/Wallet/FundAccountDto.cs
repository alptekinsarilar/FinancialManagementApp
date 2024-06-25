using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FinancialManagementApp.Attributes;

namespace FinancialManagementApp.Dto.Wallet
{
    public class FundAccountDto
    {
        [Required]
        public int AccountId { get; set; }
        [Required]
        [PositiveAmount]
        public decimal Amount { get; set; }
    }
}