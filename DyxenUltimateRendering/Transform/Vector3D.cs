using System.Numerics;

namespace DyxenUltimateRendering.Transform;

public class Vector3D(float x = 0, float y = 0, float z = 0) : Vector(x, y, z)
{
    public static ReadOnlyVector3D Zero { get; } = new(0);
    public static ReadOnlyVector3D One { get; } = new(1, 1, 1);
    public static ReadOnlyVector3D UnitX { get; } = new(1);
    public static ReadOnlyVector3D UnitY { get; } = new(y: 1);
    public static ReadOnlyVector3D UnitZ { get; } = new(z: 1);
    public float X { get => this[0]; set => this[0] = value; }
    public float Y { get => this[1]; set => this[1] = value; }
    public float Z { get => this[2]; set => this[2] = value; }
    public Vector2D XY => new(X, Y);
    public Vector2D XZ => new(X, Z);
    public Vector2D YZ => new(Y, Z);

    public static implicit operator Vector3(Vector3D vector)
    {
        return new(vector.X, vector.Y, vector.Z);
    }
    public static implicit operator Vector3D(Vector3 vector)
    {
        return new(vector.X, vector.Y, vector.Z);
    }
    public static Vector3D operator +(Vector2D a, Vector3D b)
    {
        return new(a.X + b.X, a.Y + b.Y, b.Z);
    }
    public static Vector3D operator +(Vector3D a, Vector2D b)
    {
        return b + a;
    }
    public static Vector3D operator +(Vector3D a, Vector3D b)
    {
        return new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }
    public static Vector3D operator -(Vector2D a, Vector3D b)
    {
        return new(a.X - b.X, a.Y - b.Y, b.Z);
    }
    public static Vector3D operator -(Vector3D a, Vector2D b)
    {
        return new(a.X - b.X, a.Y - b.Y, a.Z);
    }
    public static Vector3D operator -(Vector3D a, Vector3D b)
    {
        return new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }
    public static Vector3D operator *(Vector2D a, Vector3D b)
    {
        return new(a.X * b.X, a.Y * b.Y, b.Z);
    }
    public static Vector3D operator *(Vector3D a, Vector2D b)
    {
        return b * a;
    }
    public static Vector3D operator *(Vector3D a, Vector3D b)
    {
        return new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    }
    public static Vector3D operator /(Vector2D a, Vector3D b)
    {
        return new(a.X / b.X, a.Y / b.Y, b.Z);
    }
    public static Vector3D operator /(Vector3D a, Vector2D b)
    {
        return new(a.X / b.X, a.Y / b.Y, a.Z);
    }
    public static Vector3D operator /(Vector3D a, Vector3D b)
    {
        return new(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
    }
    public static Vector3D operator *(Vector3D a, float b)
    {
        return new(a.X * b, a.Y * b, a.Z * b);
    }
    public static Vector3D operator *(float a, Vector3D b)
    {
        return b * a;
    }
    public static Vector3D operator /(Vector3D a, float b)
    {
        return new(a.X / b, a.Y / b, a.Z / b);
    }
    public static Vector3D operator -(Vector3D a)
    {
        return new(-a.X, -a.Y, -a.Z);
    }
    public static Vector3D Cross(Vector3D a, Vector3D b)
    {
        return new Vector3D(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X
        );
    }
    public new Vector3D Normalize()
    {
        return base.Normalize().ToVector3D();
    }
    public new Vector3D Abs()
    {
        return base.Abs().ToVector3D();
    }
    public new Vector3D Clamp(float min, float max)
    {
        return base.Clamp(min, max).ToVector3D();
    }
    public void Deconstruct(out float x)
    {
        x = X;
    }
    public void Deconstruct(out float x, out float y)
    {
        x = X;
        y = Y;
    }
    public void Deconstruct(out float x, out float y, out float z)
    {
        x = X;
        y = Y;
        z = Z;
    }
    public static Vector3D Lerp(Vector3D v1, Vector3D v2, float t)
    {
        return Vector.Lerp(v1, v2, t).ToVector3D();
    }
    public static Vector3D Max(Vector3D v1, Vector3D v2)
    {
        return Vector.Max(v1, v2).ToVector3D();
    }
    public static Vector3D Min(Vector3D v1, Vector3D v2)
    {
        return Vector.Min(v1, v2).ToVector3D();
    }
    public static implicit operator (float x, float y, float z)(Vector3D v)
    {
        return (v.X, v.Y, v.Z);
    }
    public static implicit operator Vector3D((float, float, float) v)
    {
        return new Vector3D(v.Item1, v.Item2, v.Item3);
    }
    public Vector3D Clone()
    {
        return new(X, Y, Z);
    }
}
