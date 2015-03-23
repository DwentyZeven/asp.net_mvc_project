using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Project.Models.Properties;

namespace Project.Models.Validators
{
    public class PasswordValidatorAttribute : RegularExpressionAttribute, IClientValidatable
    {
        public PasswordValidatorAttribute() : base(Resources.PasswordValidatorRegEx)
        {
            ErrorMessage = Resources.InvalidPassword;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = Resources.InvalidPassword,
                ValidationType = "password"
            };

            rule.ValidationParameters.Add("pattern", Resources.PasswordValidatorRegEx);
            return new List<ModelClientValidationRule>() { rule };
        }
    }
}
