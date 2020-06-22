using System;
using System.Linq;
using DataMongoApi.Models;
using FluentValidation;
namespace DataMongoApi.Validation
{
    public class EmployeeFormValidator : AbstractValidator<EmployeeForm>
    {
        public EmployeeFormValidator()
        {
            RuleFor(e => e.Details.Name)
                .Length(0, 24).WithMessage("{PropertyName} Contains Invalid Characters")
                .NotEmpty().WithMessage("{Name} should not be empty")
                .BeValidName();

            RuleFor(e => e.Details.Email).EmailAddress().WithMessage("{Email} is invalid")
                .Length(0, 24).WithMessage("{PropertyName} Contains Invalid Characters")
                .NotEmpty().WithMessage("{Name} should not be empty");

        }
    }
}
