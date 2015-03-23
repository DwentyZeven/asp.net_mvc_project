using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Project.Models.Properties;

namespace Project.Models.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PastDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dateValue = value as DateTime?;
            var memberNames = new List<string>() { validationContext.MemberName };

            if (dateValue != null)
            {
                if (dateValue.Value.Date > DateTime.UtcNow.Date)
                {
                    return new ValidationResult(Resources.PastDateValidationMessage, memberNames);
                }
            }

            return ValidationResult.Success;
        }
    }
}
