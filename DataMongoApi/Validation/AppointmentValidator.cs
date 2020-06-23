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

            RuleFor(x => x.TreatmentId)
                .NotEmpty().WithMessage("{TreatmentId} must have at least one treatmentId to proceed");

            RuleFor(a => a.Notes)
                .Length(2, 150).WithMessage("Character Length is 2 - 150");

            RuleFor(a => a.Date)
                .Length(10)
                .WithMessage("{Date} should be in the formatt YYYY-MM-DD");

            RuleFor(x => x.StartTime)
                .Length(8)
                .WithMessage("{StartTime} should be in the formatt HH:MM:SS");

            RuleFor(X => X.EndTime)
                .Empty()
                .WithMessage("{EndTime} should be empty");


        }
    }
}
