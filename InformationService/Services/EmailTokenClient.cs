using InformationService.Models;
using Microsoft.Identity.Client;
using System;
using System.Threading;

namespace InformationService.Services;

public class EmailTokenClient
{
    private readonly string[] _scopes ;
    private readonly IConfidentialClientApplication _app;
    private AuthenticationResult? _token;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public EmailTokenClient(TokenClientSettings tokenClientSettings)
    {
        _scopes = tokenClientSettings.Scopes;
        _app = ConfidentialClientApplicationBuilder
            .Create(tokenClientSettings.ClientId)
            .WithClientSecret(tokenClientSettings.ClientSecret)
            .WithTenantId(tokenClientSettings.TenantId)
            .Build();
    }

    public async Task<string> GetTokenAsync()
    {
        try
        {
            await _semaphore.WaitAsync();
            if (_token == null || DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(-5)) >= _token.ExpiresOn)
            {
                _token = await _app
                    .AcquireTokenForClient(_scopes)
                    .ExecuteAsync();
            }
            return _token.AccessToken;
        }
        catch (Exception)
        {
            throw;
        }
        finally { _semaphore.Release(); }
    }
}