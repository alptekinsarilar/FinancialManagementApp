using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinancialManagementApp.Model
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("AppUser")]
        public string UserId { get; set; }
        [JsonIgnore]
        public AppUser AppUser { get; set; }

        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public Currency Currency { get; set; }
    }
}