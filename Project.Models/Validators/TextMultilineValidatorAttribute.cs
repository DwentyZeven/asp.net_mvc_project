using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Project.Models.Properties;

namespace Project.Models.Validators
{
    public class TextMultilineValidatorAttribute : RegularExpressionAttribute, IClientValidatable
    {
        public TextMultilineValidatorAttribute() : base(Resources.TextMultilineValidatorRegEx)
        {
            ErrorMessage = Resources.InvalidMultilineInput;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = Resources.InvalidMultilineInput,
                ValidationType = "textmultiline"
            };

            rule.ValidationParameters.Add("pattern", Resources.TextMultilineValidatorRegEx);
            return new List<ModelClientValidationRule>() { rule };
        }
    }
}
