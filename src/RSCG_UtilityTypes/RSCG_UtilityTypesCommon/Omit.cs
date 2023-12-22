namespace RSCG_UtilityTypesCommon;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class OmitAttribute: Attribute
{
    public OmitAttribute(string nameNewClass, params string[] omitProperties)
    {
        
    }
}
