using System.Diagnostics.CodeAnalysis;

namespace DyxenUltimateRendering.Transform;

public class Angle
{
    public const float TO_DEGREES = 180f / MathF.PI;
    public const float TO_RADIANS = MathF.PI / 180f;
    private float _radians;
    private float _degrees;
    public float Radians
    {
        get => _radians;
        set
        {
            _radians = value;
            _degrees = TO_DEGREES * _radians;
            _normalizedRadians = null;
            _normalizedDegrees = null;
        }
    }
    public float Degrees
    {
        get => _degrees;
        set
        {
            _degrees = value;
            _radians = TO_RADIANS * _degrees;
            _normalizedRadians = null;
            _normalizedDegrees = null;
        }
    }
    public float NormalizedRadians
    {
        get
        {
            if (_normalizedRadians == null)
                UpdateNormalized();
            return _normalizedRadians.Value;
        }
    }
    public float NormalizedDegrees
    {
        get
        {
            if (_normalizedDegrees == null)
                UpdateNormalized();
            return _normalizedDegrees.Value;
        }
    }
    private float? _normalizedRadians;
    private float? _normalizedDegrees;
    private Angle() 
    {
    }
    public float Sin => MathF.Sin(Radians);
    public float Cos => MathF.Cos(Radians);
    public float Tan => MathF.Tan(Radians);
    public bool IsApproximatelyDegrees(Angle a, float epsilon = 1e-6f)
    {
        return IsApproximatelyDegrees(a.Degrees, epsilon);
    }
    public bool IsApproximatelyRadians(Angle a, float epsilon = 1e-6f)
    {
        return IsApproximatelyRadians(a.Radians, epsilon);
    }
    public bool IsApproximatelyDegrees(float degrees, float epsilon = 1e-6f)
    {
        return MathF.Abs(Degrees - degrees) < epsilon;
    }
    public bool IsApproximatelyRadians(float radians, float epsilon = 1e-6f)
    {
        return MathF.Abs(Radians - radians) < epsilon;
    }
    public bool IsApproximatelyZero(float epsilon = 1e-6f)
    {
        return MathF.Abs(Radians) < epsilon;
    }
    public Angle Abs()
    {
        return FromRadians(MathF.Abs(Radians));
    }
    public static Angle Difference(Angle a, Angle b)
    {
        float difference = (b.Degrees - a.Degrees) % 360f;

        if (difference > 180f)
            difference -= 360f;

        if (difference < -180f)
            difference += 360f;

        return FromDegrees(difference);
    }
    public Angle Normalize()
    {
        return FromRadians(NormalizedRadians);
    }
    public static Angle FromRadians(float radians)
    {
        return new()
        {
            Radians = radians
        };
    }
    public static Angle FromDegrees(float degrees)
    {
        return new()
        {
            Degrees = degrees
        };
    }
    [MemberNotNull(nameof(_normalizedRadians), nameof(_normalizedDegrees))]
    private void UpdateNormalized()
    {
        _normalizedDegrees = _degrees % 360;
        if (_normalizedDegrees < 0)
            _normalizedDegrees += 360;
        _normalizedRadians = _normalizedDegrees * TO_RADIANS;
    }
    public static bool operator ==(Angle a, Angle b)
    {
        if (ReferenceEquals(a, b))
            return true;

        if (a is null || b is null)
            return false;

        return a.Radians == b.Radians;
    }
    public static bool operator !=(Angle a, Angle b)
    {
        return !(a == b);
    }
    public static Angle operator +(Angle a, Angle b)
    {
        return FromRadians(a.Radians + b.Radians);
    }
    public static Angle operator -(Angle a, Angle b)
    {
        return FromRadians(a.Radians - b.Radians);
    }
    public static Angle operator -(Angle angle)
    {
        return FromRadians(-angle.Radians);
    }
    public static Angle operator *(Angle angle, float scale)
    {
        return FromRadians(angle.Radians * scale);
    }
    public static Angle operator *(float scale, Angle angle)
    {
        return angle * scale;
    }
    public static Angle operator /(Angle angle, float scale)
    {
        return FromRadians(angle.Radians / scale);
    }
    public static implicit operator float(Angle angle)
    {
        return angle.Radians;
    }
    public static implicit operator Angle(float angle)
    {
        return FromRadians(angle);
    }
    public override int GetHashCode()
    {
        return Radians.GetHashCode();
    }
    public Angle Clone()
    {
        return FromRadians(Radians);
    }
    public bool Equals(Angle? obj)
    {
        return obj is not null && this == obj;
    }
    public override bool Equals(object? obj)
    {
        return obj is Angle a && this == a;
    }
}
