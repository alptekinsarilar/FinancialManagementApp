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
            // ToDo
            // Mock exchange rate, in real scenario this would call an external service
            return Task.FromResult(1.1m); // Mocking an exchange rate of 1.1
        }
    }
}