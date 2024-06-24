using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialManagementApp.Dto.Transaction;
using FinancialManagementApp.Interfaces;
using FinancialManagementApp.Model;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManagementApp.Controller
{
    [ApiController]
    [Route("api/transaction")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;

        public TransactionController(ITransactionRepository transactionRepository, IUserRepository userRepository)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        public IActionResult CreateTransaction(CreateTransactionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userRepository.GetById(dto.UserId);
            if (user == null)
            {
                return BadRequest(new { message = "User not found" });
            }

            var transaction = new Transaction
            {
                UserId = dto.UserId,
                Amount = dto.Amount,
                Description = dto.Description,
                Category = dto.Category,
                TransactionDate = DateTime.Now
            };

            _transactionRepository.Create(transaction);
            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
        }

        [HttpGet("{id}")]
        public IActionResult GetTransaction(int id)
        {
            var transaction = _transactionRepository.GetById(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetTransactionsByUserId(int userId)
        {
            var transactions = _transactionRepository.GetByUserId(userId);
            return Ok(transactions);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTransaction(int id, UpdateTransactionDto dto)
        {
            var transaction = _transactionRepository.GetById(id);
            if (transaction == null)
            {
                return NotFound();
            }

            transaction.Amount = dto.Amount;
            transaction.Description = dto.Description;
            transaction.Category = dto.Category;

            _transactionRepository.Update(transaction);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTransaction(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = _transactionRepository.Delete(id);

            if (stockModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}