using HappeningService.Models;
using System.ComponentModel.DataAnnotations;

namespace HappeningService.DTO
{
    public record UpdateHappeningDTO
    {
        [Required]
        public required string Guid { get; set; }

        public List<HappeningArtistIncompleteDTO> HappeningArtists { get; init; } = [];
        //public List<HappeningArtistIncompleteDTO> RemoveHappeningArtists { get; init; } = [];
    }
}
