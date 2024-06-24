using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialManagementApp.Data;
using FinancialManagementApp.Interfaces;
using FinancialManagementApp.Model;

namespace FinancialManagementApp.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDBContext _context;

        public TransactionRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public Transaction Create(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return transaction;
        }

        public Transaction GetById(int id)
        {
            return _context.Transactions.Find(id);
        }

        public IEnumerable<Transaction> GetByUserId(int userId)
        {
            return _context.Transactions.Where(t => t.UserId == userId).ToList();
        }

        public void Update(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var transaction = _context.Transactions.Find(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                _context.SaveChanges();
            }
        }
    }
}