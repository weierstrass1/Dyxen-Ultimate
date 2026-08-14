namespace DyxenUltimateRendering.Transform;

public class ReadOnlyEulerAngles(float x = 0, float y = 0, float z = 0) : EulerAngles(x, y, z)
{
    public new Angle X => base.X;
    public new Angle Y => base.Y;
    public new Angle Z => base.Z;
}
