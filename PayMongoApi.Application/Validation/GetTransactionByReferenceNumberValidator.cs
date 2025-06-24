using System;
using FluentValidation;
using PayMongo.Payment.Api.Application.Query;

namespace PayMongo.Payment.Api.Application.Validation;

public class GetTransactionByReferenceNumberValidator: AbstractValidator<GetTransactionByReferenceNumberQuery>
{
    public GetTransactionByReferenceNumberValidator()
    {
        RuleFor(x => x.ReferenceNumber)
            .NotEmpty()
            .WithMessage("Reference number is required.")
            .Length(1, 50)
            .WithMessage("Reference number must be between 1 and 50 characters long.");
    }
}
