using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Project.Models.Properties;

namespace Project.Models.Validators
{
    public class TextLineValidatorAttribute : RegularExpressionAttribute, IClientValidatable
    {
        public TextLineValidatorAttribute() : base(Resources.TextLineValidatorRegEx)
        {
            ErrorMessage = Resources.InvalidInputCharacter;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = Resources.InvalidInputCharacter,
                ValidationType = "textlineinput"
            };

            rule.ValidationParameters.Add("pattern", Resources.TextLineValidatorRegEx);
            return new List<ModelClientValidationRule>() { rule };
        }
    }
}
