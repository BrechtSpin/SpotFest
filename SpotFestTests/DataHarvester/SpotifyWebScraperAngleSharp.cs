using DataHarvester.SpotifyWeb;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotFestTests.DataHarvester;
[TestClass]
public class SpotifyWebScraperAngleSharpTest
{
    
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("3gzpq5DPGxSnKTe4SA8HAU")]
    public async Task GetListenersAsync_MisformedInput(string SpotifyUId)
    {
        //arrange
        var scraper = new SpotifyWebScraperAngleSharp();

        //act
        var output = await scraper.GetListenersAsync(SpotifyUId);

        //assert
        Assert.AreEqual(-1, output);
    }



    [TestMethod]
    [DataRow("4gzpq5DPGxSnKTe4SA8HAU")]
    public async Task GetListenersAsync_CorrectOutput(string SpotifyUId)
    {
        //arrange
        var scraper = new SpotifyWebScraperAngleSharp();

        //act
        var output1 = await scraper.GetListenersAsync(SpotifyUId);

        //assert
        Assert.IsTrue(1000000 <  output1 && output1 < 1000000000);
    }
}
