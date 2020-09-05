using System;
using DataMongoApi.Models;
using FluentValidation;

namespace DataMongoApi.Validation
{
    public class OrderDetailsValidator : AbstractValidator<OrderDetails>
    {
        public OrderDetailsValidator()
        {
            RuleFor(x => x.ClientId)
                .BeAValid24HexStringId();

            RuleFor(x => x.TreatmentOrders)
                .NotEmpty().WithMessage("{Treatments} can not be empty");

        }
    }
}
