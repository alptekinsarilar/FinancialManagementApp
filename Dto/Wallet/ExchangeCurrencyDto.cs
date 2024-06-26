using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialManagementApp.Dto.Wallet
{
    public class ExchangeCurrencyDto
    {
        public int SenderAccountId { get; set; }
        public int RecipientAccountId { get; set; }
        public decimal Amount { get; set; }
    }

}