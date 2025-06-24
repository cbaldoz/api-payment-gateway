using Microsoft.AspNetCore.Mvc;
using PayMongo.Payment.Api.DTO;
using MediatR;
using PayMongo.Payment.Api.Application.Query;
using PayMongo.Payment.Api.Middleware;
using PayMongo.Payment.Api.Application.Command;
using PayMongo.Payment.Api.Domain.Entity;
using PayMongo.Payment.Api.Infrastructure.Utility;

namespace PayMongo.Payment.Api.Controller
{
    [Route("v1/checkout/")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly AppConfigProvider _appConfigProvider;
        public CheckoutController(IMediator mediator, AppConfigProvider appConfigProvider)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _appConfigProvider = appConfigProvider ?? throw new ArgumentNullException(nameof(appConfigProvider));
        }

        /// <summary>
        /// Creates a checkout session for payment processing.
        /// This endpoint allows the client to initiate a payment session by providing the amount to be charged.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Route("session")]
        [ProducesResponseType(typeof(CheckoutSessionResponseDto), 200)]
        [HttpPost]
        [TypeFilter(typeof(ApiAuthorization))]
        public async Task<IActionResult> CreateCheckoutSession(double amount, CancellationToken cancellationToken)
        {
            var paymentMethodsResult = await _mediator.Send(new GetPaymentMethodsQuery(), cancellationToken);
            if (paymentMethodsResult == null || !paymentMethodsResult.IsSuccess)
            {
                return BadRequest("Failed to retrieve payment methods.");
            }

            var paymentMethodsTest = new List<string>
            {
                "card", // Default payment method
                "gcash", // Example additional payment method
                "qrph" // Example additional payment method
            };

            var account = HttpContext.Items["Account"] as string ?? "";
            var email = HttpContext.Items["Email"] as string ?? "";
            var accountCode = HttpContext.Items["AccountCode"] as string ?? "";

            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(email))
                return BadRequest("Account or email is missing.");

            var checkout = new CheckoutSessionDto();
            var requestToCommand = checkout.Fill(
                amount,
                email,
                account,
                paymentMethodsTest,
                _appConfigProvider
            );

            var result = await _mediator.Send(requestToCommand, cancellationToken);
            if (result.IsSuccess)
            {
                var insertData = checkout.FillTransaction(
                    requestToCommand.ReferenceNumber ?? string.Empty,
                    account,
                    amount,
                    accountCode,
                    result.Data
                );

                var isSave = await _mediator.Send(insertData, cancellationToken);

                if (isSave.Data)
                {
                    var response = new CheckoutSessionResponseDto()
                    {
                        CheckoutUrl = result.Data?.Data?.Attributes?.CheckoutUrl ?? string.Empty,
                        Amount = amount,
                        ReferenceNumber = result.Data?.Data.Attributes.ReferenceNumber ?? string.Empty
                    };
                    return Ok(response);
                }
                else
                {
                    return BadRequest("Failed to save transaction data.");
                }
            }
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Expires a checkout session based on the provided reference number.
        /// This endpoint allows the client to mark a checkout session as expired, which can be useful for cleaning up sessions that are no longer valid.
        /// </summary>
        /// <param name="refNum"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Route("session/{refNum}/expire")]
        [TypeFilter(typeof(ApiAuthorization))]
        [ProducesResponseType(typeof(bool), 200)]
        [HttpPost]
        public async Task<IActionResult> ExpiredCheckoutSession(string refNum, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ExpiredCheckoutSessionCommand() { SessionId = refNum }, cancellationToken);
            if (result.IsSuccess)
                return Ok(true);
            else
                return BadRequest(result.Message);
        }
    }
}
