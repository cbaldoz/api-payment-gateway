using System;
using AutoMapper;
using PayMongo.Payment.Api.Application.Command;
using PayMongo.Payment.Api.Domain.Entity;

namespace PayMongo.Payment.Api.Application.Mapper;

public class CommandPaymentTransactionMapper: Profile
{
    public CommandPaymentTransactionMapper()
    {
        /// <summary>
        /// Maps InsertCheckoutSessionCommand to PayMongoTransaction.
        /// </summary>
        CreateMap<InsertCheckoutSessionCommand, PayMongoTransaction>();
    }
}
