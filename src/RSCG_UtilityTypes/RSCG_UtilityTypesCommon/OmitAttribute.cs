namespace RSCG_UtilityTypesCommon;
/// <summary>
/// Generates a new class with the same name as the original class, but without the properties specified .
/// See https://www.typescriptlang.org/docs/handbook/utility-types.html
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class OmitAttribute: Attribute
{
    public OmitAttribute(string nameNewClass, params string[] omitProperties)
    {
        
    }
}
