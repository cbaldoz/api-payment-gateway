using System;
using PayMongo.Payment.Api.Domain.Entity;

namespace PayMongo.Payment.Api.Domain.Repository;

public interface IAuthenticationRepository
{
    Task<AppModel> GetAppDetailsAsync(string sessionKey, CancellationToken cancellationToken = default);
    Task<UserAccount> GetUserAccountAsync(string account, CancellationToken cancellationToken = default);
}
