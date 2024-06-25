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
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public DateTime TransferDate { get; set; } = DateTime.Now;

        public AppUser Sender { get; set; }
        public AppUser Recipient { get; set; }
    }
}