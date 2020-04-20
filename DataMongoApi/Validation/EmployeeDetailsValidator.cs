using System;
using System.Linq;
using DataMongoApi.Models;
using FluentValidation;
namespace DataMongoApi.Validation
{
    public class EmployeeDetailsValidator : AbstractValidator<EmployeeDetails>
    {
        public EmployeeDetailsValidator()
        {
            RuleFor(e => e.Name)
                .Length(0, 24).WithMessage("{PropertyName} Contains Invalid Characters")
                .NotEmpty().WithMessage("{Name} should not be empty")
                .BeValidName();

            RuleFor(e => e.Email).EmailAddress().WithMessage("{Email} is invalid")
                .Length(0, 24).WithMessage("{PropertyName} Contains Invalid Characters")
                .NotEmpty().WithMessage("{Name} should not be empty");

        }
    }
}
