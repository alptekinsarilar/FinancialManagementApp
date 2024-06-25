using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialManagementApp.Model;

namespace FinancialManagementApp.Interfaces
{
    public interface IExchangeRateService
    {
        Task<decimal> GetExchangeRateAsync(Currency fromCurrency, Currency toCurrency);
    }
}