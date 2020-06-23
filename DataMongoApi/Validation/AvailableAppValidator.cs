using System;
using DataMongoApi.Models;
using FluentValidation;

namespace DataMongoApi.Validation
{
    public class AvailableAppValidator : AbstractValidator<AvailableAppRequestForm>
    {
        public AvailableAppValidator()
        {
            RuleFor(x => x.TreatmentIds)
                .NotEmpty().WithMessage("{Treatments} can not be empty");
        }
    }
}
