using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Project.Models.Properties;

namespace Project.Models.Validators
{
    public class EmailValidatorAttribute : RegularExpressionAttribute, IClientValidatable
    {
        public EmailValidatorAttribute() : base(Resources.EmailValidatorRegEx)
        {
            ErrorMessage = Resources.InvalidEmail;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = Resources.InvalidEmail,
                ValidationType = "email"
            };

            rule.ValidationParameters.Add("pattern", Resources.EmailValidatorRegEx);
            return new List<ModelClientValidationRule>() { rule };
        }
    }
}
