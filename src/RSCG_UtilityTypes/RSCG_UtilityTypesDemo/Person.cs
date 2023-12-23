namespace RSCG_UtilityTypesDemo;

[Omit("Person", nameof(Salary))]
public class PersonWithSalary
{
    public string? LastName { get; set; }
    public string? FirstName { get; set; }

    public int Salary { get; set; }

}


