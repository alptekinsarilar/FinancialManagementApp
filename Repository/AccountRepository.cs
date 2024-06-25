using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialManagementApp.Data;
using FinancialManagementApp.Interfaces;
using FinancialManagementApp.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FinancialManagementApp.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDBContext _context;

        public AccountRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Account> CreateAccountAsync(Account account)
        {
            _context.Accounts.Add(account);
            try
            {
                await _context.SaveChangesAsync();
                return account;
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601) // Unique constraint violation
            {
                throw new InvalidOperationException("An account with the same name already exists.", ex);
            }
        }


        public async Task<Account> GetAccountByIdAsync(int accountId)
        {
            return await _context.Accounts.FindAsync(accountId);
        }


        public async Task<IEnumerable<Account>> GetAccountsByUserIdAsync(string userId)
        {
            return await _context.Accounts
                                 .Where(a => a.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<Account> UpdateAccountAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
            {
                return false;
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}