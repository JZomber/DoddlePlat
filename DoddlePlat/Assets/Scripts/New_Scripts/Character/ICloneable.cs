public interface ICloneable
{
    ICloneable Clone();
}

public static class Cloner
{
    public static ICloneable CloneableObject(ICloneable cloneable)
    {
        var newClone = cloneable.Clone();
        return default;
    }
}