using System;
using System.Linq;
using DataMongoApi.Models;
using FluentValidation;

namespace DataMongoApi.Validation
{
    public class ClientDetailsValidator : AbstractValidator<ClientDetails>
    {
        public ClientDetailsValidator()
        {
            RuleFor(c => c.FirstName)
               .BeValidLength()
               .NotEmpty().WithMessage("{First Name} should not be empty")
               .BeValidName();

            RuleFor(c => c.LastName)
               .BeValidLength()
               .NotEmpty().WithMessage("{Last Name} should not be empty")
               .BeValidName();

            RuleFor(e => e.Email).EmailAddress().WithMessage("{Email} is invalid")
                .Length(2, 30).WithMessage("{Email} length must be between 2 - 30 characters");
        }

        
    }
}
