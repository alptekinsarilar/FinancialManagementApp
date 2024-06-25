using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialManagementApp.Model;

namespace FinancialManagementApp.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> CreateAccountAsync(Account account);
        Task<Account> GetAccountByIdAsync(int accountId);
        Task<IEnumerable<Account>> GetAccountsByUserIdAsync(string userId);
        Task<Account> UpdateAccountAsync(Account account);
        Task<bool> DeleteAccountAsync(int accountId);
    }
}