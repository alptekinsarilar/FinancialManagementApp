using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialManagementApp.Data;
using FinancialManagementApp.Dto.Transaction;
using FinancialManagementApp.Dto.Wallet;
using FinancialManagementApp.Exceptions;
using FinancialManagementApp.Interfaces;
using FinancialManagementApp.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinancialManagementApp.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDBContext _context;

        public AccountService(IAccountRepository accountRepository, IExchangeRateService exchangeRateService, UserManager<AppUser> userManager, ApplicationDBContext context)
        {
            _accountRepository = accountRepository;
            _exchangeRateService = exchangeRateService;
            _userManager = userManager;
            _context = context;
        }
        

        public async Task<Account> CreateAccountAsync(AppUser user, CreateAccountDto dto)
        {
            var account = new Account
            {
                UserId = user.Id,
                AccountName = dto.AccountName,
                Currency = dto.Currency,
                Balance = 0
            };

            try
            {
                return await _accountRepository.CreateAccountAsync(account);
            }
            catch (InvalidOperationException ex)
            {
                // Handle the specific exception for duplicate account name
                throw new Exception(ex.Message);
            }
        }


        public async Task<DeleteAccountResult> DeleteAccountAsync(int accountId, string userId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account == null)
            {
                return new DeleteAccountResult { Success = false, ErrorMessage = "Account not found" };
            }

            if (account.UserId != userId)
            {
                return new DeleteAccountResult { Success = false, ErrorMessage = "User not authorized" };
            }

            if (account.Balance != 0)
            {
                return new DeleteAccountResult { Success = false, ErrorMessage = "Account cannot be deleted because it has a non-zero balance" };
            }

            var success = await _accountRepository.DeleteAccountAsync(accountId);
            return new DeleteAccountResult { Success = success };
        }


        public async Task<IEnumerable<Account>> GetAccountsByUserIdAsync(string userId)
        {
            return await _accountRepository.GetAccountsByUserIdAsync(userId);
        }

        public async Task<bool> FundAccountAsync(FundAccountDto dto, string userId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(dto.AccountId);
            if (account == null || account.UserId != userId)
            {
                return false;
            }

            account.Balance += dto.Amount;

            // Create a transaction entry
            var transaction = new Transaction
            {
                AccountId = account.Id,
                Amount = dto.Amount,
                Description = "Account Funding",
                Category = "Funding",
                TransactionDate = DateTime.Now,
                CreatedAt = DateTime.Now
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return await _accountRepository.UpdateAccountAsync(account) != null;
        }

        public async Task<bool> WithdrawFromAccountAsync(WithdrawAccountDto dto, string userId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(dto.AccountId);
            if (account == null || account.UserId != userId)
            {
                return false;
            }

            if (account.Balance < dto.Amount)
            {
                return false;
            }

            account.Balance -= dto.Amount;

            // Create a transaction entry
            var transaction = new Transaction
            {
                AccountId = account.Id,
                Amount = -dto.Amount,
                Description = "Account Withdrawal",
                Category = "Withdrawal",
                TransactionDate = DateTime.Now,
                CreatedAt = DateTime.Now
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return await _accountRepository.UpdateAccountAsync(account) != null;
        }


        public async Task<TransferResult> TransferFundsAsync(TransferFundsDto dto, string userId)
        {
            using (var dbContextTransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var senderAccount = await _context.Accounts.FindAsync(dto.SenderAccountId);
                    var recipientAccount = await _context.Accounts.FindAsync(dto.RecipientAccountId);

                    if (senderAccount == null || recipientAccount == null)
                    {
                        return new TransferResult { Success = false, ErrorMessage = "Invalid account" };
                    }

                    if (senderAccount.UserId != userId)
                    {
                        return new TransferResult { Success = false, ErrorMessage = "Unauthorized" };
                    }

                    if (senderAccount.Currency != recipientAccount.Currency)
                    {
                        return new TransferResult { Success = false, ErrorMessage = "Accounts have different currencies" };
                    }

                    if (senderAccount.Balance < dto.Amount)
                    {
                        return new TransferResult { Success = false, ErrorMessage = "Insufficient funds" };
                    }

                    // Deduct amount from sender's account
                    senderAccount.Balance -= dto.Amount;
                    _context.Accounts.Update(senderAccount);

                    // Add amount to recipient's account
                    recipientAccount.Balance += dto.Amount;
                    _context.Accounts.Update(recipientAccount);

                    // Create transaction entries
                    var senderTransaction = new Transaction
                    {
                        AccountId = senderAccount.Id,
                        Amount = -dto.Amount,
                        Description = $"Transfer to account {recipientAccount.Id}",
                        Category = "Transfer",
                        TransactionDate = DateTime.Now,
                        CreatedAt = DateTime.Now
                    };

                    var recipientTransaction = new Transaction
                    {
                        AccountId = recipientAccount.Id,
                        Amount = dto.Amount,
                        Description = $"Transfer from account {senderAccount.Id}",
                        Category = "Transfer",
                        TransactionDate = DateTime.Now,
                        CreatedAt = DateTime.Now
                    };

                    _context.Transactions.Add(senderTransaction);
                    _context.Transactions.Add(recipientTransaction);

                    var transfer = new Transfer
                    {
                        SenderAccountId = senderAccount.Id,
                        RecipientAccountId = recipientAccount.Id,
                        Amount = dto.Amount,
                        TransferDate = DateTime.Now,
                        SenderAccount = senderAccount,
                        RecipientAccount = recipientAccount
                    };

                    _context.Transfers.Add(transfer);


                    await _context.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();

                    return new TransferResult { Success = true };
                }
                catch (Exception ex)
                {
                    await dbContextTransaction.RollbackAsync();
                    return new TransferResult { Success = false, ErrorMessage = ex.Message };
                }
            }
        }


        public async Task<bool> ExchangeCurrencyAsync(int accountId, string targetCurrency, string userId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account == null || account.UserId != userId)
            {
                return false;
            }

            if (!Enum.TryParse(typeof(Currency), targetCurrency, true, out var targetCurrencyEnum))
            {
                throw new ArgumentException("Invalid currency type");
            }

            var exchangeRate = await _exchangeRateService.GetExchangeRateAsync(account.Currency, (Currency)targetCurrencyEnum);
            account.Balance *= exchangeRate;
            account.Currency = (Currency)targetCurrencyEnum;

            return await _accountRepository.UpdateAccountAsync(account) != null;
        }

        public async Task<(bool, List<ResponseTransactionDto>)> GetTransactionsByAccountIdAsync(int accountId, string userId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null || account.UserId != userId)
            {
                return (false, null);
            }

            var transactions = await _context.Transactions
                                            .Where(t => t.AccountId == accountId)
                                            .OrderByDescending(t => t.TransactionDate)
                                            .Select(t => new ResponseTransactionDto
                                            {
                                                Id = t.Id,
                                                AccountId = t.AccountId,
                                                Amount = t.Amount,
                                                Description = t.Description,
                                                Category = t.Category,
                                                TransactionDate = t.TransactionDate,
                                                CreatedAt = t.CreatedAt
                                            })
                                            .ToListAsync();

            return (true, transactions);
        }

    }
}