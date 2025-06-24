using System;
using AutoMapper;
using FluentValidation;
using MediatR;
using PayMongo.Payment.Api.Application.Command;
using PayMongo.Payment.Api.Domain.Entity;
using PayMongo.Payment.Api.Domain.Repository;

namespace PayMongo.Payment.Api.Application.CommandHandler;

public class InsertCheckoutSessionCommandHandler : IRequestHandler<InsertCheckoutSessionCommand, Result<bool>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;
    public InsertCheckoutSessionCommandHandler(IPaymentRepository paymentRepository,
    IMapper mapper)
    {
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Handles the insertion of a new checkout session into the payment repository.
    /// This method maps the incoming command to a PayMongoTransaction entity and saves it to the repository.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<bool>> Handle(InsertCheckoutSessionCommand request, CancellationToken cancellationToken)
    {
        var paymentTransaction = _mapper.Map<PayMongoTransaction>(request);

        var result = await _paymentRepository.SavePaymentAsync(paymentTransaction, cancellationToken);

        return result.result
            ? Result<bool>.Success(true, "Checkout session inserted successfully.")
            : Result<bool>.Failure(result.message ?? "Failed to insert checkout session.");
    }
}
