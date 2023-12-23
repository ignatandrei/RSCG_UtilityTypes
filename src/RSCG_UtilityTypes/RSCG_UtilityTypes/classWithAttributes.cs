
using RSCG_UtilityTypes;
namespace RSCG_UtilityTypes;

record ClassWithAttributes: IEqualityComparer<ClassWithAttributes>
{
    public readonly ClassDeclarationSyntax? cds;
    public readonly ImmutableArray<AttributeData> attributes;
    public readonly string NameClass=string.Empty;
    public readonly bool HasOneEmptyConstructor=false;
    public ClassWithAttributes(ClassDeclarationSyntax? cds,ImmutableArray<AttributeData> attributes )
    {
        this.cds = cds;
        this.attributes = attributes;
        if (this.cds != null)
        {
            var ctors = this.cds.Members
                .OfType<ConstructorDeclarationSyntax>().ToArray();
            if((ctors?.Length??0) == 0)
            {
                this.HasOneEmptyConstructor = true;
            }
            else
            {
                this.HasOneEmptyConstructor =
                    ctors!.Any(it => it.ParameterList.Parameters.Count == 0);
            }
            this.NameClass = this.cds.Identifier.ToString();
        }
    }

    public bool Equals(ClassWithAttributes x, ClassWithAttributes y)
    {
        if( x?.cds == y?.cds) return true;

        return false;
    }

    public int GetHashCode(ClassWithAttributes obj)
    {
        if(obj.cds == null) return 0;
        return obj.cds.GetHashCode();
    }
    string NameSpaceName()
    {
        var namespaceDeclaration = cds!.Parent;
        while (namespaceDeclaration is not null)
        {
            if (namespaceDeclaration is BaseNamespaceDeclarationSyntax nsds)
            {
                return nsds.Name.ToString();
                
            }
            namespaceDeclaration = namespaceDeclaration.Parent;
        }
        return "";
    }
    
    PropertyDeclarationSyntax[]? AllPublicProperties()
    {
        var props = cds!
            .Members
            .OfType<PropertyDeclarationSyntax>()
            .ToArray();
        if (props.Length == 0) return null; 
        var publicProps = props
                .Where(it => it.Modifiers.Any(SyntaxKind.PublicKeyword))
                .ToArray();
        if (publicProps.Length == 0) return null ;

        return publicProps;

    }
    public Dictionary<AttributeData,GeneratedDataFromAttribute> Generate()
    {
        Dictionary<AttributeData, GeneratedDataFromAttribute> ret = new();
        foreach (var attribute in attributes)
        {
            GeneratedDataFromAttribute? code;
            if (!this.HasOneEmptyConstructor)
            {
                code= new GeneratedDataFromAttribute(GenerationResult.NoEmptyConstructor);
            }
            else
            {
                code = Generate(attribute);
            }
            
            ret.Add(attribute, code);
        }
        return ret;
    }
    public GeneratedDataFromAttribute Generate(AttributeData data)
    {
        if (data is null) return new GeneratedDataFromAttribute(GenerationResult.NotFoundAttribute);
        var typeAttribute = FunctionalType.None;
        var nameAttribute =data.AttributeClass?.Name.ToString();
        if(nameAttribute == "OmitAttribute")
        {
            typeAttribute = FunctionalType.Omit;
        }
        else if (nameAttribute == "PickAttribute")
        {
            typeAttribute = FunctionalType.Pick;
        }
        if(FunctionalType.None == typeAttribute) 
        {
            return new GeneratedDataFromAttribute(GenerationResult.NotFoundFunctionalType);
        };

        var nameClassFromAttribute = data.ConstructorArguments[0].Value?.ToString()??"";
        var propsAttr = data
            .ConstructorArguments[1]
            .Values
            .Select(it=>it.Value?.ToString())
            .Where(it=>!string.IsNullOrWhiteSpace(it ))
            .ToArray();

        var props = AllPublicProperties();
        if (props is null) return new GeneratedDataFromAttribute(GenerationResult.NoPublicPropertiesInClass);
        switch(typeAttribute)
        {
            case FunctionalType.Omit:
                props = props
                    .Where(it => !propsAttr.Contains(it.Identifier.ToString()))
                    .ToArray();
                break;
            case FunctionalType.Pick:
                props = props
                    .Where(it => propsAttr.Contains(it.Identifier.ToString()))
                    .ToArray();
                break;
            default:
                return new GeneratedDataFromAttribute(GenerationResult.NotFoundFunctionalType);
        }

        var sb = new StringBuilder();
        string namespaceName = NameSpaceName();
        if (namespaceName.Length > 0)
        {
            sb.AppendLine($"namespace {namespaceName}");
            sb.AppendLine("{");
        }

        sb.AppendLine($"partial class {nameClassFromAttribute}");
        sb.AppendLine("    {");
        foreach (var prop in props)
        {
            sb.AppendLine($"        public {prop.Type} {prop.Identifier} {{ get; set; }}");
          
        }
        string attr = 
            string.Join("\r\n", props
            .Select(prop=>$"ret.{prop.Identifier} = data.{prop.Identifier};")
            .ToArray());
        var conv = @$"
public static explicit operator {nameClassFromAttribute}({this.NameClass} data )
    {{
        var ret= new {nameClassFromAttribute} ();
        {attr}
        return ret;
    }}

";
        sb.AppendLine(conv);
        conv = @$"
public static explicit operator {this.NameClass}({nameClassFromAttribute} data )
    {{
        var ret= new {this.NameClass} ();
        {attr}
        return ret;
    }}

";
        sb.AppendLine(conv);

        sb.AppendLine("}");
        if (namespaceName.Length>0)
        {
            sb.AppendLine("}");

        }
        return new GeneratedDataFromAttribute(sb.ToString(), nameClassFromAttribute);
    }
}
