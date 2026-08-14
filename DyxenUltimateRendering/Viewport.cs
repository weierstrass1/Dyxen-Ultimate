using DyxenUltimateRendering.Interfaces;

namespace DyxenUltimateRendering;

public class Viewport : IPosition, ISize2D
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; }
    public int Height { get; }
}
