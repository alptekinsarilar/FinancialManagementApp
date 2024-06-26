using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialManagementApp.Interfaces;
using FinancialManagementApp.Model;

namespace FinancialManagementApp.Service
{
    public class MockExchangeRateService : IExchangeRateService
    {
        public Task<decimal> GetExchangeRateAsync(Currency fromCurrency, Currency toCurrency)
        {
            decimal exchangeRate;

            if (fromCurrency == Currency.TRY && toCurrency == Currency.USD)
            {
                exchangeRate = 0.033m; // TRY to USD
            }
            else if (fromCurrency == Currency.USD && toCurrency == Currency.TRY)
            {
                exchangeRate = 30m; // USD to TRY
            }
            else
            {
                throw new ArgumentException("Exchange rate not available for the specified currencies");
            }

            return Task.FromResult(exchangeRate);
        }
    }

}