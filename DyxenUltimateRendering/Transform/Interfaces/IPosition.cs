namespace DyxenUltimateRendering.Transform.Interfaces;

public interface IPosition<T> where T : Vector
{
    public T Position { get; set; }
}
