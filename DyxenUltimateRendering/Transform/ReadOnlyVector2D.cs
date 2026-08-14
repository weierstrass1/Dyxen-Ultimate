namespace DyxenUltimateRendering.Transform;

public class ReadOnlyVector2D(float x = 0, float y = 0) : Vector2D(x, y)
{
    public new float X => base.X;
    public new float Y => base.Y;
}
