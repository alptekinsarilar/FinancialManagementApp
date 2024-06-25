using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialManagementApp.Model
{
    public class Transfer
    {
        public int Id { get; set; }
        public int SenderAccountId { get; set; }
        public int RecipientAccountId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public DateTime TransferDate { get; set; } = DateTime.Now;

        public Account SenderAccount { get; set; }
        public Account RecipientAccount { get; set; }
    }

}