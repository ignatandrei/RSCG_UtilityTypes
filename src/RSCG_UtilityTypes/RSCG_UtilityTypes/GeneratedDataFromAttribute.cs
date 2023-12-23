
namespace RSCG_UtilityTypes;

class GeneratedDataFromAttribute
{
    public readonly string code;
    public readonly string? nameClass;
    public readonly GenerationResult generationResult;

    public GeneratedDataFromAttribute(string code, string nameClass)
        : this(code, GenerationResult.Succes)
    {
        this.nameClass = nameClass;
    }
    public GeneratedDataFromAttribute(GenerationResult generationResult)
        : this("", generationResult)
    {
    }
    private GeneratedDataFromAttribute(string code, GenerationResult generationResult)
    {
        this.code = code;
        this.generationResult = generationResult;
    }
}
