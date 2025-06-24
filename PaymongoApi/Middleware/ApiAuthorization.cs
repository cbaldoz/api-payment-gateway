using System;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PayMongo.Payment.Api.Application.Service;

namespace PayMongo.Payment.Api.Middleware;

public class ApiAuthorization : Attribute, IAuthorizationFilter
{
    private IApplicationService _applicationService;
    public ApiAuthorization(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var request = context.HttpContext.Request;
        var authorization = request.Headers["Authorization"];

        AuthenticationHeaderValue authenticationHeaderValue = new AuthenticationHeaderValue("Bearer",
            authorization.ToString().Replace("Bearer ", ""));


        if (authenticationHeaderValue == null || authenticationHeaderValue.Scheme != "Bearer")
        {
            var temp = new ObjectResult(new { message = "INVALID KEY" });
            temp.StatusCode = 401;
            context.Result = temp;
            return;
        }
        if (string.IsNullOrEmpty(authenticationHeaderValue.Parameter))
        {
            var temp = new ObjectResult(new { message = "401-INVALID KEY" });
            temp.StatusCode = 401;
            context.Result = temp;
            return;
        }
        string sessionKey = authorization.ToString().Replace("Bearer ", "");
        var appModel = _applicationService.GetSessionAsync(sessionKey).Result;

        if (appModel == null)
        {
            var temp = new ObjectResult(new { message = "401-INVALID KEY" });
            temp.StatusCode = 401;
            context.Result = temp;
            return;
        }
        if (Convert.ToDateTime(appModel.SessionExpiry) < DateTime.UtcNow)
        {
            var temp = new ObjectResult(new { message = "401-INVALID KEY" });
            temp.StatusCode = 401;
            context.Result = temp;
            return;
        }

        using JsonDocument doc = JsonDocument.Parse(appModel.RequestDetails);
        JsonElement root = doc.RootElement;

        string? account = null;
        if (root.TryGetProperty("investorAccounts", out JsonElement investorAccountsElem) &&
            investorAccountsElem.ValueKind == JsonValueKind.Array &&
            investorAccountsElem.GetArrayLength() > 0)
        {
            var firstAccountElem = investorAccountsElem[0];
            if (firstAccountElem.TryGetProperty("account", out JsonElement accountElem) &&
                accountElem.ValueKind == JsonValueKind.String)
            {
                account = accountElem.GetString();
            }
            var userAccount = _applicationService.GetUserAccountAsync(account).Result;
            context.HttpContext.Items["Account"] = account;
            context.HttpContext.Items["Email"] = appModel.Username;
            context.HttpContext.Items["AccountCode"] = userAccount?.AccountCode ?? string.Empty;
        }
        else
        {
            var temp = new ObjectResult(new { message = "401-INVALID KEY" });
            temp.StatusCode = 401;
            context.Result = temp;
        }
    }
}