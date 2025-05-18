namespace HappeningService.DTO
{
    public record HappeningSummaryDTO
    {
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public required DateOnly StartDate { get; set; }
        public required DateOnly EndDate { get; set; }
    }
}
