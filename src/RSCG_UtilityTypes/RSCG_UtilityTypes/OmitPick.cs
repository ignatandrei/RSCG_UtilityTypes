
namespace RSCG_UtilityTypes;
[Generator]
public class OmitPick : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classes = context.SyntaxProvider
            .ForAttributeWithMetadataName(
            "RSCG_UtilityTypesCommon.OmitAttribute",
                predicate: static (node, _) =>
                {
                    var isCds = (node is ClassDeclarationSyntax);
                    return isCds;
                },
                transform: static (syntaxContext, _) =>
                {
                    var cds = syntaxContext.TargetNode as ClassDeclarationSyntax;
                    var omits = syntaxContext.Attributes;
                    if (cds is null) return null;
                    return new ClassWithAttributes(cds, omits);
                }


            )
            .Collect()
            .SelectMany((it, _) => it.Distinct());


        context.RegisterSourceOutput(classes, Execute);

    }

    private void Execute(SourceProductionContext context, ClassWithAttributes? source)
    {
        if (source?.cds is null ) return;
        var x = source;
        var y = x.cds.Kind();
        var z=y.ToString();

    }
  }
