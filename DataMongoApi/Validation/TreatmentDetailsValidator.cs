using System;
using System.Linq;
using DataMongoApi.Models;
using FluentValidation;

namespace DataMongoApi.Validation
{
    public class TreatmentDetailsValidator : AbstractValidator<TreatmentDetails>
    {
        public TreatmentDetailsValidator()
        {
            RuleFor(t => t.TreatmentName)
               .BeValidLength()
               .NotEmpty().WithMessage("{Name} should not be empty")
               .BeValidName();

            RuleFor(t => t.TreatmentType)
               .BeValidLength()
               .NotEmpty().WithMessage("{Type} should not be empty")
               .BeValidName();

            RuleFor(t => t.Price)
                .NotEmpty().WithMessage("{Price} can not be empty")
                .Must(BeMoreThanZero).WithMessage("{{Price} must be more than 0");

            RuleFor(t => t.Duration)
                .NotEmpty().WithMessage("{Duration} can not be empty")
                .Must(BeMoreThanZero).WithMessage("{Duration} must be more than zero");
        }

        protected bool BeMoreThanZero(int price)
        {
            return price > 0;
        }
    }
}
