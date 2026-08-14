using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace DyxenUltimateRendering.Transform;

public class Vector
{
    public int Dimension { get; }
    public float this[int index]
    {
        get => _parameters[index];
        set
        {
            _parameters[index] = value;
            MakeDirty();
        }
    }
    private Lazy<float> _magnitude;
    private Lazy<float> _magnitude2;
    public float Magnitude { get => _magnitude.Value; }
    public float MagnitudeSquared { get => _magnitude2.Value; }
    private readonly float[] _parameters;
    public Vector(params float[] parameters)
    {
        _parameters = parameters;
        Dimension = parameters.Length;
        MakeDirty();
    }
    public Vector Normalize()
    {
        return this / Magnitude;
    }
    public Vector Abs()
    {
        return new([.. _parameters.Select(Math.Abs)]);
    }
    public float Dot(Vector vector)
    {
        int dimension = Math.Min(Dimension, vector.Dimension);
        float result = 0;

        for (int i = 0; i < dimension; i++)
            result += this[i] * vector[i];

        return result;
    }
    public float Distance(Vector vector)
    {
        return (this - vector).Magnitude;
    }
    public float DistanceSquared(Vector vector)
    {
        return (this - vector).MagnitudeSquared;
    }
    public Vector Clamp(float min, float max)
    {
        return new([.. _parameters.Select(f => Math.Clamp(f, min, max))]);
    }
    public static Vector Lerp(Vector v1, Vector v2, float t)
    {
        return v1 + ((v2 - v1) * t);
    }
    public static Vector Max(Vector v1, Vector v2)
    {
        return new(Operate(v1, v2, Math.Max, float.NegativeInfinity, float.NegativeInfinity));
    }
    public static Vector Min(Vector v1, Vector v2)
    {
        return new(Operate(v1, v2, Math.Min, float.PositiveInfinity, float.PositiveInfinity));
    }
    public bool EqualsPadded(Vector vector, float padding = 0)
    {
        if (!EqualsValues(vector))
            return false;
        (Vector higher, Vector lower) = Dimension < vector.Dimension ?
            (vector, this) :
            (this, vector);
        for (int i = lower.Dimension; i < higher.Dimension; i++)
        {
            if (higher[i] != padding)
                return false;
        }
        return true;
    }
    public bool EqualsValues(Vector vector)
    {
        int dimension = Math.Min(Dimension, vector.Dimension);
        for (int i = 0; i < dimension; i++)
        {
            if (this[i] != vector[i])
                return false;
        }
        return true;
    }
    public override bool Equals(object? obj)
    {
        if (obj is not Vector v)
            return false;
        return Equals(v);
    }
    public bool Equals(Vector? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;
        if (obj is null)
            return false;
        if (Dimension != obj.Dimension)
            return false;
        return EqualsValues(obj);
    }
    public override int GetHashCode()
    {
        HashCode hash = new();
        hash.Add(Dimension);
        foreach (float parameter in _parameters)
            hash.Add(parameter);

        return hash.ToHashCode();
    }
    public Vector2D ToVector2D()
    {
        return Dimension switch
        {
            0 => new(),
            1 => new(this[0]),
            _ => new(this[0], this[1]),
        };
    }
    public Vector3D ToVector3D()
    {
        return Dimension switch
        {
            0 => new(),
            1 => new(this[0]),
            2 => new(this[0], this[1]),
            _ => new(this[0], this[1], this[2]),
        };
    }
    public static implicit operator Vector<float>(Vector vector)
    {
        return new(vector._parameters);
    }
    public static Vector operator +(Vector a, Vector b)
    {
        return new(Operate(a, b, static (f1, f2) => f1 + f2));
    }
    public static Vector operator -(Vector a, Vector b)
    {
        return new(Operate(a, b, static (f1, f2) => f1 - f2));
    }
    public static Vector operator *(Vector a, Vector b)
    {
        return new(Operate(a, b, static (f1, f2) => f1 * f2, 1, 1));
    }
    public static Vector operator /(Vector a, Vector b)
    {
        return new(Operate(a, b, static (f1, f2) => f1 / f2, 0, 1));
    }
    public static Vector operator *(Vector a, float b)
    {
        float[] parameters = [.. a._parameters.Select(f => f * b)];
        return new(parameters);
    }
    public static Vector operator *(float a, Vector b)
    {
        return b * a;
    }
    public static Vector operator /(Vector a, float b)
    {
        float[] parameters = [.. a._parameters.Select(f => f / b)];
        return new(parameters);
    }
    public static Vector operator -(Vector a)
    {
        float[] parameters = [.. a._parameters.Select(f => -f)];
        return new(parameters);
    }
    public static bool operator ==(Vector a, Vector b)
    {
        return a.Equals(b);
    }
    public static bool operator !=(Vector a, Vector b)
    {
        return !a.Equals(b);
    }
    private static float[] Operate<T>(T a, T b, Func<float, float, float> op, float neutralElement1 = 0, float neutralElement2 = 0) where T : Vector
    {
        int dim = Math.Max(a.Dimension, b.Dimension);
        float[] parameters = new float[dim];
        for (int i = 0; i < dim; i++)
        {
            float av = i < a.Dimension ? a[i] : neutralElement1;
            float bv = i < b.Dimension ? b[i] : neutralElement2;

            parameters[i] = op(av, bv);
        }
        return parameters;
    }
    private float CalculateMagnitude()
    {
        return (float)Math.Sqrt(MagnitudeSquared);
    }
    private float CalculateMagnitude2()
    {
        float sum = 0;
        foreach (float parameter in _parameters)
            sum += parameter * parameter;

        return sum;
    }
    [MemberNotNull(nameof(_magnitude), nameof(_magnitude2))]
    private void MakeDirty()
    {
        _magnitude = new(CalculateMagnitude, true);
        _magnitude2 = new(CalculateMagnitude2, true);
    }
}
