namespace RSCG_UtilityTypes;
[Generator]
public class OmitPick : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        Func<SyntaxNode, CancellationToken, bool> predicate = static (node, _) =>
                        {
                            var isCds = (node is ClassDeclarationSyntax);
                            return isCds;
                        };
        Func<GeneratorAttributeSyntaxContext, CancellationToken, ClassWithAttributes?> transform = static (syntaxContext, _) =>
                        {
                            var cds = syntaxContext.TargetNode as ClassDeclarationSyntax;
                            var omits = syntaxContext.Attributes;
                            if (cds is null) return null;
                            return new ClassWithAttributes(cds, omits);
                        };
        var classesOmit = context.SyntaxProvider
            .ForAttributeWithMetadataName(
            "RSCG_UtilityTypesCommon.OmitAttribute",
                predicate: predicate,
                transform: transform
            )
            .Collect()
            .SelectMany((it, _) => it.Distinct());

        var classesPick = context.SyntaxProvider
            .ForAttributeWithMetadataName(
            "RSCG_UtilityTypesCommon.PickAttribute",
                predicate: predicate,
                transform: transform
            )
            .Collect()
            .SelectMany((it, _) => it.Distinct());


        context.RegisterSourceOutput(classesOmit, Execute);
        context.RegisterSourceOutput(classesPick, Execute);

    }

    private void Execute(SourceProductionContext spc, ClassWithAttributes? source)
    {
        if (source?.cds is null) return;
        string nameClass = source.cds.Identifier.ToString();
        foreach (var item in source.Generate())
        {
            var result = item.Value.generationResult;

            switch (result)
            {
                case GenerationResult.Succes:

                    spc.AddSource(nameClass +"_"+ item.Value.nameClass, item.Value.code);
                    break;
                default:
                    DiagnosticDescriptor descriptor = new(
id: "RSCG_UtilityTypes"+ result,
title: "RSCG_UtilityTypes" + result,
messageFormat: "error RSCG_UtilityTypes " + result,
category: "RSCG_UtilityTypes",
DiagnosticSeverity.Error,
isEnabledByDefault: true);
                    spc.ReportDiagnostic(Diagnostic.Create(descriptor, source.cds.GetLocation()));
                    break;
            }
        }





    }
}
