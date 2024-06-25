using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialManagementApp.Attributes
{
    public class PositiveAmountAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is decimal amount)
            {
                if (amount <= 0)
                {
                    return new ValidationResult("The amount must be positive.");
                }
            }
            return ValidationResult.Success;
        }
    }
}