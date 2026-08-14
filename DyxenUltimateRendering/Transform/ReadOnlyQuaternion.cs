namespace DyxenUltimateRendering.Transform;

public class ReadOnlyQuaternion(float x = 0, float y = 0, float z = 0, float w = 0) : Quaternion(x, y, z, w)
{
    public new float X => base.X;
    public new float Y => base.Y;
    public new float Z => base.Z;
    public new float W => base.W;
}
