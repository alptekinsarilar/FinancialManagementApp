using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialManagementApp.Model;

namespace FinancialManagementApp.Interfaces
{
    public interface ITransactionRepository
    {
        Transaction Create(Transaction transaction);
        Transaction GetById(int id);
        IEnumerable<Transaction> GetByUserId(int userId);
        void Update(Transaction transaction);
        void Delete(int id);
    }
}