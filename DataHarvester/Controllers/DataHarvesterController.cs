﻿using DataHarvester.Models;
using DataHarvester.SpotifyWeb;
using Microsoft.AspNetCore.Mvc;

namespace DataHarvester.Controllers;

[ApiController]
[Route("artist")]
public class DataHarvesterController(ISpotifyWebApiClient webClient) : ControllerBase
{
    readonly ISpotifyWebApiClient _webClient = webClient;

    [HttpGet("search")]
    public async Task<IActionResult> GetArtistsByName([FromQuery]  string name)
    {
        var message = await _webClient.GetArtistsByNameAsync(name);
        var result = message.Select(artist => new DTO.ArtistDto
        {
            Name = artist.Name,
            SpotifyId = artist.Id,
        });

        return Ok(result);
    }

}
