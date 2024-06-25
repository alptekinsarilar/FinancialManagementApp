using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialManagementApp.Dto.Transaction;
using FinancialManagementApp.Dto.Wallet;
using FinancialManagementApp.Extensions;
using FinancialManagementApp.Interfaces;
using FinancialManagementApp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManagementApp.Controller
{
    [ApiController]
    [Route("api/account")]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<AppUser> _userManager;

        public WalletController(IAccountService accountService, UserManager<AppUser> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }

        private async Task<AppUser> GetCurrentUserAsync()
        {
            var username = User.GetUsername();
            return await _userManager.FindByNameAsync(username);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized(new { message = "User not found" });
            }

            try
            {   
                var account = await _accountService.CreateAccountAsync(user, dto);
                return Ok(new AccountDto
                {
                    Id = account.Id,
                    AccountName = account.AccountName,
                    Balance = account.Balance,
                    Currency = account.Currency
                });
            }
            catch (Exception ex) when (ex.Message.Contains("An account with the same name already exists."))
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the account", error = ex.Message });
            }
        }

        [HttpDelete("{accountId}")]
        public async Task<IActionResult> DeleteAccount(int accountId)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized(new { message = "User not found" });
            }

            try
            {
                var success = await _accountService.DeleteAccountAsync(accountId, user.Id);
                if (!success)
                {
                    return NotFound(new { message = "Account not found or user not authorized" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the account", error = ex.Message });
            }
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccountsByUser()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized(new { message = "User not found" });
            }

            try
            {
                var accounts = await _accountService.GetAccountsByUserIdAsync(user.Id);
                var accountDtos = accounts.Select(a => new AccountDto
                {
                    Id = a.Id,
                    AccountName = a.AccountName,
                    Balance = a.Balance,
                    Currency = a.Currency
                }).ToList();
                return Ok(accountDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching accounts", error = ex.Message });
            }
        }


        [HttpPost("fund")]
        public async Task<IActionResult> FundAccount(FundAccountDto dto)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized(new { message = "User not found" });
            }

            try
            {
                var success = await _accountService.FundAccountAsync(dto, user.Id);
                if (!success)
                {
                    return BadRequest(new { message = "Unable to fund account" });
                }

                return Ok(new { message = "Account funded successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while funding the account", error = ex.Message });
            }
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> WithdrawFromAccount(WithdrawAccountDto dto)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized(new { message = "User not found" });
            }

            try
            {
                var success = await _accountService.WithdrawFromAccountAsync(dto, user.Id);
                if (!success)
                {
                    return BadRequest(new { message = "Unable to withdraw from account. Check if you have enough balance." });
                }

                return Ok(new { message = "Withdrawal successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while withdrawing from the account", error = ex.Message });
            }
        }


        [HttpPost("transfer")]
        public async Task<IActionResult> TransferFunds(TransferFundsDto dto)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized(new { message = "User not found" });
            }

            try
            {
                var success = await _accountService.TransferFundsAsync(dto, user.Id);
                if (!success)
                {
                    return BadRequest(new { message = "Insufficient funds or unauthorized" });
                }

                return Ok(new { message = "Transfer successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while transferring funds", error = ex.Message });
            }
        }

        [HttpPost("exchange")]
        public async Task<IActionResult> ExchangeCurrency([FromQuery] int accountId, [FromBody] string targetCurrency)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized(new { message = "User not found" });
            }

            try
            {
                var success = await _accountService.ExchangeCurrencyAsync(accountId, targetCurrency, user.Id);
                if (!success)
                {
                    return BadRequest(new { message = "Unable to exchange currency" });
                }

                return Ok(new { message = "Currency exchanged successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while exchanging currency", error = ex.Message });
            }
        }

        [HttpGet("transactions/{accountId}")]
        public async Task<IActionResult> GetTransactions(int accountId)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized(new { message = "User not found" });
            }

            var (isAuthorized, transactions) = await _accountService.GetTransactionsByAccountIdAsync(accountId, user.Id);
            if (!isAuthorized)
            {
                return BadRequest(new { message = "Invalid account or unauthorized access" });
            }

            return Ok(transactions);
        }

    }
}