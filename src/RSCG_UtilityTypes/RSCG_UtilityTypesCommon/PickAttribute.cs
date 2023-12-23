namespace RSCG_UtilityTypesCommon;
/// <summary>
/// Generates a new class with the same name as the original class, but with the properties specified .
/// See https://www.typescriptlang.org/docs/handbook/utility-types.html
/// </summary>

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class PickAttribute : Attribute
{
    public PickAttribute(string nameNewClass, params string[] pickProperties)
    {

    }
}
