using System;
using System.Collections.Generic;
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

        public static IRuleBuilderOptions<T, string> BeAllNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(m => m.Replace(" ", "").Replace("-", "").All(char.IsDigit)).WithMessage("{Phone} must be all digits");
        }

        public static IRuleBuilderOptions<T, string> BeAValid24HexStringId<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(m => m.Length == 24).WithMessage("{ID} must be length 24 Characters");
        }

        public static IRuleBuilderOptions<T, IList<string>> BeAValid24HexStringId<T>(this IRuleBuilder<T, IList<string>> ruleBuilder)
        {
            return ruleBuilder.Must(m => m.Any(x => x.Length == 24)).WithMessage("{ID} must be length 24 Characters");
        }
    }
}
