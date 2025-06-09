using Microsoft.Extensions.Logging;

using DataHarvester.Models;
using DataHarvester.SpotifyWeb;
using Microsoft.Extensions.DependencyInjection;

namespace SpotFestTests.DataHarvester;
[TestClass]
public class SpotifyWebApiClientTokenClientTests
{
    private ServiceProvider Services = null!;
    private SpotifyWebApiClientTokenClient _tokenClient = null!;

    [TestInitialize]
    public void Initialize()
    {
        Services = new ServiceCollection()
            .AddLogging(logger => logger.AddDebug())
            .AddHttpClient()
            .BuildServiceProvider();

        _tokenClient = new SpotifyWebApiClientTokenClient(Services.GetService<ILoggerFactory>()!.CreateLogger<SpotifyWebApiClientTokenClient>(),
            Services.GetService<HttpClient>()!
            );
    }

    [TestMethod]
    public async Task GetTokenAsync_ReturnsNewValidToken()
    {
        //Act
        var token = await _tokenClient.GetTokenAsync();

        // Assert
        Assert.IsNotNull(token);
        Assert.IsTrue(token.Access_token.Length > 0);
        Assert.IsTrue(token.Token_type.Length > 0);
        Assert.IsTrue(token.Expires_in == 3600);
        Assert.IsNotNull(token.Expires_at);
        Assert.IsTrue(token.Expires_at >= DateTime.UtcNow && token.Expires_at <= DateTime.UtcNow + TimeSpan.FromSeconds(3600 - 30));
    }

    [TestMethod]
    public async Task GetTokenAsync_RenewsToken_WhenExpired()
    {
        //Act
        var FirstToken = await _tokenClient.GetTokenAsync();
        FirstToken.Expires_at = DateTime.MinValue;
        //System.Threading.Thread.Sleep(1000);
        var SecondToken = await _tokenClient.GetTokenAsync();
        //Assert
        Assert.AreNotEqual(FirstToken, SecondToken);
    }

    [TestMethod]
    public async Task GetTokenAsync_ReusesValidToken_WhenNotExpired()
    {
        //Act
        var FirstToken = await _tokenClient.GetTokenAsync();
        var SecondToken = await _tokenClient.GetTokenAsync();
        //Assert
        Assert.AreEqual(FirstToken, SecondToken);
    }
}