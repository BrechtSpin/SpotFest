using DataHarvester.Models;
using System.Text.Json.Serialization;


// Needed for AOT-serialization
[JsonSerializable(typeof(SpotifyWebApiClientToken))]
[JsonSerializable(typeof(SpotifyArtist))]
[JsonSerializable(typeof(SpotifyArtistResponse))]
internal partial class SerializerContext : JsonSerializerContext { }
