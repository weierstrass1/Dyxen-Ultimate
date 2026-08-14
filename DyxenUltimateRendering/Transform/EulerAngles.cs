namespace DyxenUltimateRendering.Transform;

public class EulerAngles(float x = 0, float y = 0, float z = 0)
{
    public static ReadOnlyEulerAngles Zero { get; } = new();
    public static ReadOnlyEulerAngles One { get; } = new(1, 1, 1);
    private Angle _x = x;
    private Angle _y = y;
    private Angle _z = z;
    public Angle X
    {
        get => _x;
        set
        {
            _x = value;
            _magnitudeR = null;
            _magnitudeR2 = null;
            _magnitudeD = null;
            _magnitudeD2 = null;
        }
    }
    public Angle Y
    {
        get => _y;
        set
        {
            _y = value;
            _magnitudeR = null;
            _magnitudeR2 = null;
            _magnitudeD = null;
            _magnitudeD2 = null;
        }
    }
    public Angle Z
    {
        get => _z;
        set
        {
            _z = value;
            _magnitudeR = null;
            _magnitudeR2 = null;
            _magnitudeD = null;
            _magnitudeD2 = null;
        }
    }
    private float? _magnitudeR;
    private float? _magnitudeR2;
    private float? _magnitudeD;
    private float? _magnitudeD2;
    public float MagnitudeRadians { get => _magnitudeR ??= CalculateMagnitudeRadians(); }
    public float MagnitudeSquaredRadians { get => _magnitudeR2 ??= CalculateMagnitudeRadians2(); }
    public float MagnitudeDegrees { get => _magnitudeD ??= CalculateMagnitudeDegrees(); }
    public float MagnitudeSquaredDegrees { get => _magnitudeD2 ??= CalculateMagnitudeDegrees2(); }
    public EulerAngles Clone()
    {
        return new(X, Y, Z);
    }
    public EulerAngles Normalize()
    {
        return new(
            X.Normalize(),
            Y.Normalize(),
            Z.Normalize()
        );
    }
    public bool IsApproximatelyZero(float epsilon = 1e-6f)
    {
        return X.IsApproximatelyZero(epsilon)
            && Y.IsApproximatelyZero(epsilon)
            && Z.IsApproximatelyZero(epsilon);
    }
    private float CalculateMagnitudeRadians()
    {
        return (float)Math.Sqrt(MagnitudeSquaredRadians);
    }
    private float CalculateMagnitudeRadians2()
    {
        return (X.Radians * X.Radians) + (Y.Radians * Y.Radians) + (Z.Radians * Z.Radians);
    }
    private float CalculateMagnitudeDegrees()
    {
        return (float)Math.Sqrt(MagnitudeSquaredDegrees);
    }
    private float CalculateMagnitudeDegrees2()
    {
        return (X.Degrees * X.Degrees) + (Y.Degrees * Y.Degrees) + (Z.Degrees * Z.Degrees);
    }
    public void Deconstruct(out Angle x, out Angle y, out Angle z)
    {
        x = X;
        y = Y;
        z = Z;
    }
    public static EulerAngles operator +(EulerAngles a, EulerAngles b)
    {
        return new(
            a.X + b.X,
            a.Y + b.Y,
            a.Z + b.Z
        );
    }
    public static EulerAngles operator -(EulerAngles a, EulerAngles b)
    {
        return new(
            a.X - b.X,
            a.Y - b.Y,
            a.Z - b.Z
        );
    }
    public static EulerAngles operator -(EulerAngles value)
    {
        return new(
            -value.X,
            -value.Y,
            -value.Z
        );
    }
    public static EulerAngles operator *(EulerAngles value, float scale)
    {
        return new(
            value.X * scale,
            value.Y * scale,
            value.Z * scale
        );
    }
    public static EulerAngles operator *(float scale, EulerAngles value)
    {
        return value * scale;
    }
    public static EulerAngles operator /(EulerAngles value, float scale)
    {
        return new(
            value.X / scale,
            value.Y / scale,
            value.Z / scale
        );
    }
    public static bool operator ==(EulerAngles? a, EulerAngles? b)
    {
        if (ReferenceEquals(a, b))
            return true;

        if (a is null || b is null)
            return false;

        return a.X == b.X
            && a.Y == b.Y
            && a.Z == b.Z;
    }
    public static bool operator !=(EulerAngles? a, EulerAngles? b)
    {
        return !(a == b);
    }
    public override bool Equals(object? obj)
    {
        return obj is EulerAngles other && this == other;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }
    public static implicit operator EulerAngles(Vector3D value)
    {
        return new(value.X, value.Y, value.Z);
    }

    public static implicit operator Vector3D(EulerAngles value)
    {
        return new(value.X, value.Y, value.Z);
    }
}
