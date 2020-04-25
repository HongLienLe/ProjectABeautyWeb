using System;
using DataMongoApi.Models;
using FluentValidation;

namespace DataMongoApi.Validation
{
    public class AppointmentValidator : AbstractValidator<AppointmentDetails>
    {
        public AppointmentValidator()
        {
            RuleFor(a => a.Client.FirstName)
               .BeValidLength()
               .NotEmpty().WithMessage("{First Name} should not be empty")
               .BeValidName();

            RuleFor(a => a.Client.LastName)
               .BeValidLength()
               .NotEmpty().WithMessage("{Last Name} should not be empty")
               .BeValidName();

            RuleFor(a => a.Client.Email).EmailAddress().WithMessage("{Email} is invalid")
                .Length(2, 30).WithMessage("{Email} length must be between 2 - 30 characters");

            RuleFor(a => a.Notes)
                .Length(2, 150).WithMessage("Character Length is 2 - 150");
        }
    }
}
