using System;
using System.Linq;
using FluentValidation;

namespace DataMongoApi.Validation
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, string> NotStartWithWhiteSpace<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(m => m != null && !m.StartsWith(" ")).WithMessage("'{PropertyName}' should not start with whitespace");
        }

        public static IRuleBuilderOptions<T, string> NotEndWithWhiteSpace<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(m => m != null && !m.EndsWith(" ")).WithMessage("'{PropertyName}' should not end with whitespace");
        }

        public static IRuleBuilderOptions<T,string> BeValidName<T>(this IRuleBuilder<T,string> ruleBuilder)
        {
            return ruleBuilder.Must(m => m.Replace(" ", "").Replace("-", "").All(char.IsLetter)).WithMessage("{PropertyName} Contains Invalid Characters");
        }

        public static IRuleBuilderOptions<T, string> BeValidLength<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Length(0,24).WithMessage("{PropertyName} Contains Invalid Characters");
        }
    }
}
