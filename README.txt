Omit and Pick from TypeScript . See documentation at 

https://www.typescriptlang.org/docs/handbook/utility-types.html#omittype-keys

For a class use Omit and Pick declarations

[Omit("MoviePreviewSmall", nameof(Actors),nameof(Year))]
[Pick("MoviePreviewMinimal", nameof(Title), nameof(Year))]
public class Movie
{
    public string? Title { get; set; }
    public string? Director { get; set; }
    public int Year { get; set; }
    public string[]? Actors { get; set; }
}

See more at https://github.com/ignatandrei/RSCG_UtilityTypes