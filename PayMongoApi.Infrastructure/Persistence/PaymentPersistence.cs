using System;
using Dapper;
using MySql.Data.MySqlClient;
using PayMongo.Payment.Api.Domain.Entity;
using PayMongo.Payment.Api.Domain.Repository;
using PayMongo.Payment.Api.Infrastructure.Utility;

namespace PayMongo.Payment.Api.Infrastructure.Persistence;

public class PaymentPersistence : IPaymentRepository
{
    private readonly ConnStringConfig _connStringConfig;
    public PaymentPersistence(AppConfigProvider appConfigProvider)
    {
        _connStringConfig = appConfigProvider.ConnStringConfig;
    }

    /// <summary>
    /// Retrieves a PayMongo event by its reference number.
    /// This method queries the payment.paymongo_event table to find an event associated with the given transaction ID.
    /// </summary>
    /// <param name="referenceNumber"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<(PayMongoEvent PayMongoEvent, string message)> GetPayMongoEventByReferenceNumber(string referenceNumber, CancellationToken cancellationToken)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        var query = @"SELECT * FROM payment.paymongo_event WHERE transaction_id = @referenceNumber";
        using (var conn = new MySqlConnection(_connStringConfig.PaymentDbConnection))
        {
            await conn.OpenAsync(cancellationToken);
            var queryResult = await conn.QueryFirstOrDefaultAsync<PayMongoEvent>(query, new
            {
                referenceNumber
            });
            if (queryResult != null)
                return (queryResult, "Payment event retrieved successfully.");
            else
                return (null, "No payment event found.");
        }
    }

    /// <summary>
    /// Retrieves payment transactions based on the provided filter.
    /// The filter can include various criteria such as date range, status, etc.
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<(List<PaymentTransaction> transactions, string message)> GetPaymentTransactionAsync(
        int limit,
        DateTime startDate,
        DateTime endDate,
        string status,
        string transactionType,
        string account,
        CancellationToken cancellationToken = default)
    {
        var sql = @"SELECT 
                        t.*,
                        pe.event_id,
                        pe.event_type,
                        pe.raw_payload,
                        pe.received_at
                    FROM 
                        transaction t
                    LEFT JOIN
                        paymongo_event pe
                    ON 
                        t.transaction_id = pe.transaction_id
                    WHERE 
                        account = @Account";
        var parameters = new DynamicParameters();
        parameters.Add("Account", account);
        if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
        {
            sql += " AND DATE(session_created_at) BETWEEN @StartDate AND @EndDate";
            parameters.Add("StartDate", startDate);
            parameters.Add("EndDate", endDate);
        }

        if (!string.IsNullOrEmpty(status))
        {
            sql += " AND status = @Status";
            parameters.Add("Status", status);
        }

        if (!string.IsNullOrEmpty(transactionType))
        {
            sql += " AND transaction_type = @TransactionType";
            parameters.Add("TransactionType", transactionType);
        }

        sql += " ORDER BY session_created_at DESC";

        if (limit > 0)
        {
            sql += " LIMIT @Limit";
            parameters.Add("Limit", limit);
        }

        using (var conn = new MySqlConnection(_connStringConfig.PaymentDbConnection))
        {
            await conn.OpenAsync(cancellationToken);
            var queryResult = await conn.QueryAsync<PaymentTransaction>(sql, parameters);
            if (queryResult.Any())
                return (queryResult.ToList(), "Payment transactions retrieved successfully.");
            else
                return (new List<PaymentTransaction>(), "No payment transactions found.");
        }
    }

    /// <summary>
    /// Saves a payment transaction to the database.
    /// This method inserts a new payment transaction into the payment database.
    /// </summary>
    /// <param name="payment"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<(bool result, string message)> SavePaymentAsync(PayMongoTransaction payment, CancellationToken cancellationToken = default)
    {
        var query = @"
            INSERT INTO transaction 
                (transaction_id,
                session_id,
                payment_intent_id,
                client_key,
                account,
                account_code,
                amount,
                currency,
                status ,
                transaction_type,
                transaction_created_at,
                transaction_updated_at,
                checkout_url,
                session_created_at,
                session_expires_at,
                credit_status,
                reconciled_status) 
            VALUES
                (@transaction_id,
                @session_id,
                @payment_intent_id,
                @client_key,
                @account,
                @account_code,
                @amount,
                @currency,
                @status,
                @transaction_type,
                @transaction_created_at,
                @transaction_updated_at,
                @checkout_url,
                @session_created_at,
                @session_expires_at,
                @credit_status,
                @reconciled_status)";
        using (var conn = new MySqlConnection(_connStringConfig.PaymentDbConnection))
        {
            await conn.OpenAsync(cancellationToken);
            var transaction = await conn.BeginTransactionAsync(cancellationToken);
            var queryResult = await conn.ExecuteAsync(query, new
            {
                transaction_id = payment.TransactionId,
                session_id = payment.SessionId,
                payment_intent_id = payment.PaymentIntentId,
                client_key = payment.ClientKey,
                account = payment.Account,
                account_code = payment.AccountCode,
                amount = payment.Amount,
                currency = payment.Currency,
                status = payment.Status,
                transaction_type = payment.TransactionType,
                transaction_created_at = payment.TransactionCreatedAt,
                transaction_updated_at = payment.TransactionUpdatedAt,
                checkout_url = payment.CheckoutUrl,
                session_created_at = payment.SessionCreatedAt,
                session_expires_at = payment.SessionExpiresAt,
                credit_status = payment.CreditStatus,
                reconciled_status = payment.ReconciledStatus
            }, transaction);
            if (queryResult > 0)
            {
                await transaction.CommitAsync(cancellationToken);
                return (true, "Payment saved successfully.");
            }
            else
            {
                await transaction.RollbackAsync(cancellationToken);
                return (false, "Failed to save payment.");
            }
        }
    }

    /// <summary>
    /// Updates the payment status of a transaction.
    /// This method modifies the status of an existing payment transaction in the database.
    /// </summary>
    /// <param name="paymentStatus"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<(bool result, string message)> UpdatePaymentStatusAsync(PayMongoTransaction paymentStatus, CancellationToken cancellationToken = default)
    {
        var query = @"
            UPDATE transaction SET 
                status = @status 
            WHERE
                transaction_id = @transaction_id";

        using (var conn = new MySqlConnection(_connStringConfig.PaymentDbConnection))
        {
            await conn.OpenAsync(cancellationToken);
            var transaction = await conn.BeginTransactionAsync(cancellationToken);
            var queryResult = await conn.ExecuteAsync(query, new
            {
                status = paymentStatus.Status,
                transaction_id = paymentStatus.TransactionId,
            }, transaction);
            if (queryResult > 0)
            {
                await transaction.CommitAsync(cancellationToken);
                return (true, "Payment Status updated successfully.");
            }
            else
            {
                await transaction.RollbackAsync(cancellationToken);
                return (false, "Failed to update payment status.");
            }
        }
    }

    /// <summary>
    /// Retrieves a payment transaction by its reference number.
    /// This method queries the payment.transaction table to find a transaction associated with the given reference number.
    /// </summary>
    /// <param name="referenceNumber"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<(PaymentTransaction? transaction, string message)> GetTransactionByReferenceNumber(string referenceNumber, CancellationToken cancellationToken)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        var query = @"SELECT 
                        t.*,
                        pe.event_id,
                        pe.event_type,
                        pe.raw_payload,
                        pe.received_at
                    FROM 
                        transaction t
                    LEFT JOIN
                        paymongo_event pe
                    ON 
                        t.transaction_id = pe.transaction_id
                    WHERE 
                        t.transaction_id = @referenceNumber";

        using (var conn = new MySqlConnection(_connStringConfig.PaymentDbConnection))
        {
            await conn.OpenAsync(cancellationToken);
            return await conn.QueryFirstOrDefaultAsync<PaymentTransaction>(query, new
            {
                referenceNumber
            })
                .ContinueWith(task =>
                {
                    if (task.Result != null)
                        return (task.Result, "Payment transaction retrieved successfully.");
                    else
                        return (null, "No payment transaction found.");
                }, cancellationToken);
        }
    }
}
