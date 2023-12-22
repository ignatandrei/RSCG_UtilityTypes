namespace RSCG_UtilityTypes;
internal record ClassWithAttributes: IEqualityComparer<ClassWithAttributes>
{
    public readonly ClassDeclarationSyntax? cds;
    public readonly ImmutableArray<AttributeData> attributes;

    public ClassWithAttributes(ClassDeclarationSyntax? cds,ImmutableArray<AttributeData> attributes )
    {
        this.cds = cds;
        this.attributes = attributes;
    }

    public bool Equals(ClassWithAttributes x, ClassWithAttributes y)
    {
        if( x?.cds == y?.cds) return true;

        return false;
    }

    public int GetHashCode(ClassWithAttributes obj)
    {
        throw new NotImplementedException();
    }
}
