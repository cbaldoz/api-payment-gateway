using System;
using MediatR;

namespace PayMongo.Payment.Api.Application.Command;

public class ExpiredCheckoutSessionCommand : IRequest<Result<bool>>
{
    public string SessionId { get; set; } = string.Empty;
}
