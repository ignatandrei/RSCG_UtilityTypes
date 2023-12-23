namespace RSCG_UtilityTypesDemo;

[Omit("MoviePreviewSmall", nameof(Actors),nameof(Year))]
[Pick("MoviePreviewMinimal", nameof(Title), nameof(Year))]
public class Movie
{
    public string? Title { get; set; }
    public string? Director { get; set; }
    public int Year { get; set; }
    public string[]? Actors { get; set; }
}


