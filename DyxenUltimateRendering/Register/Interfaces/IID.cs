namespace DyxenUltimateRendering.Register.Interfaces;
public interface IID
{
    public Guid ID { get; }
    public static T CreateInstance<T>(IDManager idManager) where T : IID<T>
    {
        return T.CreateInstance(idManager);
    }
}
public interface IID<T> : IID where T : IID<T>
{
    public static abstract T CreateInstance(IDManager idManager);
    internal static abstract T CreateInstance(Guid id);
}
