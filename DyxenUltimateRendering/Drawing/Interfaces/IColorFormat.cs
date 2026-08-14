namespace DyxenUltimateRendering.Drawing.Interfaces;

public interface IColorFormat<TEnum> where TEnum : Enum
{
    public IEnumerable<ColorChannel<TEnum>> Channels { get; }
}
