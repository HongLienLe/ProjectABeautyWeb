using System;
using DataMongoApi.Models;
using FluentValidation;

namespace DataMongoApi.Validation
{
    public class OperatingHoursDetailsValidatior : AbstractValidator<OperatingHoursDetails>
    {
        public OperatingHoursDetailsValidatior()
        {

            RuleFor(d => d.OpeningHr)
                .NotEmpty().WithMessage("{Opening Time} can not be empty")
                .Must(isCorrectFormatToParseToTimeSpan).WithMessage("{Opening Hr} is not in the correct formatt {HH:MM:SS}");

            RuleFor(d => d.ClosingHr)
                .NotEmpty().WithMessage("{Opening Time} can not be empty")
                .Must(isCorrectFormatToParseToTimeSpan).WithMessage("{Closing Hr} is not in the correct formatt {HH:MM:SS}");

        }

        protected bool isCorrectFormatToParseToTimeSpan(string hour)
        {
            TimeSpan timeSpan;
            return TimeSpan.TryParse(hour,out timeSpan );
        }
    }

}
