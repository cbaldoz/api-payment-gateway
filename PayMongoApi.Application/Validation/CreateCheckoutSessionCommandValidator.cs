using System;
using PayMongo.Payment.Api.Application.Command;
using FluentValidation;

namespace PayMongo.Payment.Api.Application.Validation;

public class CreateCheckoutSessionCommandValidator: AbstractValidator<CreateCheckoutSessionCommand>
{
    public CreateCheckoutSessionCommandValidator()
    {
        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage("Amount is required.")
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0.");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithMessage("Currency is required.")
            .Length(3)
            .WithMessage("Currency must be 3 characters long.");

        RuleFor(x => x.PaymentMethodTypes)
            .NotEmpty()
            .WithMessage("Payment method types are required.");
    }
}
