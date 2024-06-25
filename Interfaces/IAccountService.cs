using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialManagementApp.Dto.Transaction;
using FinancialManagementApp.Dto.Wallet;
using FinancialManagementApp.Exceptions;
using FinancialManagementApp.Model;

namespace FinancialManagementApp.Interfaces
{
    public interface IAccountService
    {
        Task<Account> CreateAccountAsync(AppUser user, CreateAccountDto dto);
        Task<bool> DeleteAccountAsync(int accountId, string userId);
        Task<IEnumerable<Account>> GetAccountsByUserIdAsync(string userId);
        Task<bool> FundAccountAsync(FundAccountDto dto, string userId);
        Task<bool> WithdrawFromAccountAsync(WithdrawAccountDto dto, string userId);
        Task<TransferResult> TransferFundsAsync(TransferFundsDto dto, string userId);
        Task<bool> ExchangeCurrencyAsync(int accountId, string targetCurrency, string userId);
        Task<(bool, List<ResponseTransactionDto>)> GetTransactionsByAccountIdAsync(int accountId, string userId);
    }
}