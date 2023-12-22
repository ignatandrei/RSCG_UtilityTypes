namespace RSCG_UtilityTypesDemo;

[Omit("Person", nameof(Salary))]
public class PersonWithSalary
{
    public string? LastName { get; set; }
    public string? FirstName { get; set; }

    public int Salary { get; set; }

    public string FullName => $"{FirstName} {LastName}";

}

[Omit("MoviePreviewSmall", nameof(Actors),nameof(Year))]
[Omit("MoviePreviewMinimal", nameof(Actors), nameof(Director))]
public class Movie
{
    public string? Title { get; set; }
    public string? Director { get; set; }
    public int Year { get; set; }
    public string[]? Actors { get; set; }
}


