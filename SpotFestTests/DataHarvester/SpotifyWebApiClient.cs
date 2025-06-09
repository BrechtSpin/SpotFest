using DataHarvester.SpotifyWeb;
using Microsoft.Extensions.DependencyInjection;

namespace SpotFestTests.DataHarvester
{
    [TestClass]
    public class SpotifyWebApiClientTests
    {
        private ServiceProvider Services = null!;
        private SpotifyWebApiClient _client = null!;

        [TestInitialize]
        public void Initialize()
        {

            Services = new ServiceCollection()
                .AddSingleton<SpotifyWebApiClientTokenClient>()
                .AddSingleton<SpotifyRateLimiter>()
                .AddHttpClient()
                .BuildServiceProvider();

            _client = new SpotifyWebApiClient(Services.GetService<HttpClient>()!,
                Services.GetService<SpotifyWebApiClientTokenClient>()!,
                Services.GetService<SpotifyRateLimiter>()!
                );
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("3gzpq5DPGxSnKTe4SA8HAU3gzpq5DPGxSnKTe4SA8HAU")]
        public async Task GetArtistAsync_MalformedKeyInput(string SpotifyUId)
        {
            //assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _client.GetArtistAsync(SpotifyUId));
        }

        [TestMethod]
        [DataRow("3gzpq5DPGxSnKTe4SA8HAU")]
        public async Task GetArtistAsync_MissingKeyInput(string SpotifyUId)
        {
            //assert
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await _client.GetArtistAsync(SpotifyUId));
        }

        [TestMethod]
        [DataRow("4gzpq5DPGxSnKTe4SA8HAU")]
        public async Task GetArtistAsync_Correctinput(string SpotifyUId)
        {
            //act
            var output = await _client.GetArtistAsync(SpotifyUId);
            //assert
            Assert.AreEqual("Coldplay", output.Name);
        }
    }
}
