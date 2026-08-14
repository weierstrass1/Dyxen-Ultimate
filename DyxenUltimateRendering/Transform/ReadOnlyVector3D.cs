namespace DyxenUltimateRendering.Transform;

public class ReadOnlyVector3D(float x = 0, float y = 0, float z = 0) : Vector3D(x, y, z)
{
    public new float X { get => base.X; }
    public new float Y { get => base.Y; }
    public new float Z { get => base.Z; }
}
