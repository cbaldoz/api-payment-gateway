using System;
using PayMongo.Payment.Api.Domain.Entity;
using PayMongo.Payment.Api.Domain.Repository;

namespace PayMongo.Payment.Api.Application.Service;

public interface IApplicationService
{
    Task<AppModel> GetSessionAsync(string sessionKey, CancellationToken cancellationToken = default);
    Task<UserAccount> GetUserAccountAsync(string account, CancellationToken cancellationToken = default);
}
public class ApplicationService : IApplicationService
{
    private readonly IAuthenticationRepository _authenticationRepository;
    public ApplicationService(IAuthenticationRepository authenticationRepository)
    {
        _authenticationRepository = authenticationRepository ?? throw new ArgumentNullException(nameof(authenticationRepository));
    }
    public async Task<AppModel> GetSessionAsync(string sessionKey, CancellationToken cancellationToken = default)
    {
        return await _authenticationRepository.GetAppDetailsAsync(sessionKey, cancellationToken);
    }

    public async Task<UserAccount> GetUserAccountAsync(string account, CancellationToken cancellationToken = default)
    {
        return await _authenticationRepository.GetUserAccountAsync(account, cancellationToken);
    }
}
