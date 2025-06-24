using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayMongo.Payment.Api.Application.Query;
using PayMongo.Payment.Api.Domain.Entity;
using PayMongo.Payment.Api.DTO;
using PayMongo.Payment.Api.Middleware;

namespace PayMongo.Payment.Api.Controller
{
    [Route("v1/")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves a list of payment transactions based on the specified filters.
        /// This endpoint allows the client to fetch payment transactions within a specified date range, with optional filters for status and transaction type.
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="status"></param>
        /// <param name="transactionType"></param>
        /// <returns></returns>
        [Route("transactions")]
        [ProducesResponseType(typeof(List<PaymentResponseDto>), 200)]
        [HttpGet]
        [TypeFilter(typeof(ApiAuthorization))]
        public async Task<IActionResult> GetTransactions(int limit,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken,
            string? status = null,
            string? transactionType = null)
        {
            var paymentFilter = new GetPaymentTransactionQuery
            {
                Limit = limit,
                StartDate = startDate,
                EndDate = endDate,
                Status = status ?? string.Empty,
                TransactionType = transactionType ?? string.Empty,
                Account = HttpContext.Items["Account"] as string ?? ""
            };
            var result = await _mediator.Send(paymentFilter, cancellationToken);
            if (result.IsSuccess)
                return Ok(new PaymentResponseDto().Fill(result.Data));
            else
                return Ok(new List<PaymentTransaction>());
        }


        /// <summary>
        /// Retrieves a payment transaction by its reference number.
        /// This endpoint allows the client to fetch a specific payment transaction using its unique reference number.
        /// </summary>
        /// <param name="referenceNumber"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Route("transactions/{referenceNumber}")]
        [HttpGet]
        [TypeFilter(typeof(ApiAuthorization))]
        [ProducesResponseType(typeof(PaymentResponseDto), 200)]
        public async Task<IActionResult> GetTransactionByReferenceNumber(string referenceNumber, CancellationToken cancellationToken)
        {
            var paymentFilter = new GetTransactionByReferenceNumberQuery
            {
                ReferenceNumber = referenceNumber
            };
            var result = await _mediator.Send(paymentFilter, cancellationToken);
            if (result.IsSuccess)
                return Ok(new PaymentResponseDto().Fill(result.Data));
            else
                return NotFound();
        }
    }
}
