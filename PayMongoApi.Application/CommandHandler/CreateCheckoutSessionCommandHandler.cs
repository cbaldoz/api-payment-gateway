using System;
using System.Xml.XPath;
using PayMongo.Payment.Api.Domain.Entity;
using MediatR;
using PayMongo.Payment.Api.Application.Command;
using PayMongo.Payment.Api.Domain.Repository;
using FluentValidation;
using AutoMapper;

namespace PayMongo.Payment.Api.Application.CommandHandler;

public class CreateCheckoutSessionCommandHandler : IRequestHandler<CreateCheckoutSessionCommand, Result<CheckoutSession>>
{
    private readonly IPayMongoRepository _payMongoRepository;
    private readonly IValidator<CreateCheckoutSessionCommand> _validator;
    private readonly IMapper _mapper;
    public CreateCheckoutSessionCommandHandler(IPayMongoRepository payMongoRepository,
    IValidator<CreateCheckoutSessionCommand> validator,
    IMapper mapper)
    {
        _payMongoRepository = payMongoRepository ?? throw new ArgumentNullException(nameof(payMongoRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    /// <summary>
    /// This method handles the CreateCheckoutSessionCommand to create a new checkout session.
    /// It validates the request using FluentValidation and maps the command to a CheckoutSession entity.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<CheckoutSession>> Handle(CreateCheckoutSessionCommand request, CancellationToken cancellationToken)
    {
        var validResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validResult.IsValid)
        {
            return Result<CheckoutSession>.Failure(validResult.ToString());
        }

        var checkoutSession = _mapper.Map<CheckoutSession>(request);

        var result = await _payMongoRepository.CreateCheckoutSession(checkoutSession, cancellationToken);

        if (result.result is null)
            return Result<CheckoutSession>.Failure(result.message ?? "Failed to create checkout session.");
        else
            return Result<CheckoutSession>.Success(result.result, "Checkout session created successfully.");
    }
}
