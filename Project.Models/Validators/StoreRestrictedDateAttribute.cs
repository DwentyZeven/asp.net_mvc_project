using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using Project.Models.Properties;

namespace Project.Models.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class StoreRestrictedDateAttribute : ValidationAttribute
    {
        private static readonly DateTime minumumDateTime = SqlDateTime.MinValue.Value;
        private static readonly DateTime maximumDateTime = SqlDateTime.MaxValue.Value;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dateValue = value as DateTime?;
            var memberNames = new List<string>() { validationContext.MemberName };

            if (dateValue != null)
            {
                if ((dateValue.Value.Date < minumumDateTime) ||
                    (dateValue.Value.Date > maximumDateTime))
                {
                    return new ValidationResult(string.Format(Resources.StoreRestrictedDateValidationMessage, 
                                                minumumDateTime, maximumDateTime, memberNames));
                }
            }

            return ValidationResult.Success;
        }
    }
}
