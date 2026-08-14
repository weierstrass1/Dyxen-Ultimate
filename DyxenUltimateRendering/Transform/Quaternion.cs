namespace DyxenUltimateRendering.Transform;

public class Quaternion
{
    public static ReadOnlyQuaternion Zero { get; } = new();
    public static ReadOnlyQuaternion One { get; } = new(1, 1, 1, 1);
    public static ReadOnlyQuaternion Identity { get; } = new(w: 1);
    public Vector3D Forward => this * Vector3D.UnitZ;
    public Vector3D Up => this * Vector3D.UnitY;
    public Vector3D Right => this * Vector3D.UnitX;
    private float _x, _y, _z, _w;
    private EulerAngles? _eulerAngles;
    public EulerAngles EulerAngles
    {
        get
        {
            if(_eulerAngles == null)
            {
                float sinPitch = 2f * (W * X + Y * Z);
                float cosPitch = 1f - 2f * (X * X + Y * Y);
                float pitch = MathF.Atan2(sinPitch, cosPitch);

                float sinYaw = 2f * (W * Y - Z * X);
                float yaw;

                if (MathF.Abs(sinYaw) >= 1f)
                    yaw = MathF.CopySign(MathF.PI / 2f, sinYaw);
                else
                    yaw = MathF.Asin(sinYaw);

                float sinRoll = 2f * (W * Z + X * Y);
                float cosRoll = 1f - 2f * (Y * Y + Z * Z);
                float roll = MathF.Atan2(sinRoll, cosRoll);
                _eulerAngles = new(pitch, yaw, roll);
            }
            return _eulerAngles.Clone();
        }
    }
    public float X
    {
        get => _x;
        set
        {
            _x = value;
            MakeDirty();
        }
    }
    public float Y
    {
        get => _y;
        set
        {
            _y = value;
            MakeDirty();
        }
    }
    public float Z
    {
        get => _z;
        set
        {
            _z = value;
            MakeDirty();
        }
    }
    public float W
    {
        get => _w;
        set
        {
            _w = value;
            MakeDirty();
        }
    }
    private float? _magnitude;
    private float? _magnitude2;
    public float Magnitude { get => _magnitude ??= CalculateMagnitude(); }
    public float MagnitudeSquared { get => _magnitude2 ??= CalculateMagnitude2(); }
    public Quaternion(float x = 0, float y = 0, float z = 0, float w = 0)
    {
        Set(x, y, z, w);
    }
    public void Set(float x = 0, float y = 0, float z = 0, float w = 0)
    {
        _x = x;
        _y = y;
        _z = z;
        _w = w;
        MakeDirty();
    }
    private float CalculateMagnitude()
    {
        return (float)Math.Sqrt(MagnitudeSquared);
    }
    private float CalculateMagnitude2()
    {
        return (X * X) + (Y * Y) + (Z * Z) + (W * W);
    }
    private void MakeDirty()
    {
        _magnitude = null;
        _magnitude2 = null;
        _eulerAngles = null;
    }
    public Quaternion Normalize()
    {
        return this / Magnitude;
    }
    public Quaternion Conjugate()
    {
        return new(-X, -Y, -Z, W);
    }
    public Quaternion Inverse()
    {
        return Conjugate() / MagnitudeSquared;
    }
    public Vector3D Rotate(Vector3D v)
    {
        return this * v;
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
    public void Deconstruct(out float x, out float y, out float z, out float w)
    {
        x = X;
        y = Y;
        z = Z;
        w = W;
    }
    public static float Dot(Quaternion a, Quaternion b)
    {
        return (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z) + (a.W * b.W);
    }
    public static Angle AngleBetween(Quaternion a, Quaternion b)
    {
        a = a.Normalize();
        b = b.Normalize();

        float dot = MathF.Abs(Dot(a, b));

        dot = Math.Clamp(dot, -1f, 1f);

        return Angle.FromRadians(
            2f * MathF.Acos(dot)
        );
    }
    public static Quaternion Lerp(Quaternion a, Quaternion b, float t)
    {
        a = a.Normalize();
        b = b.Normalize();

        if (Dot(a, b) < 0)
            b = -b;

        return (a * (1f - t) + b * t).Normalize();
    }
    public static Quaternion LerpClamped(Quaternion a, Quaternion b, float t)
    {
        return Lerp(a, b, Math.Clamp(t, 0, 1));
    }
    public static Quaternion SLerp(Quaternion a, Quaternion b, float t)
    {
        a = a.Normalize();
        b = b.Normalize();
        float dot = Dot(a, b);

        if (dot < 0f)
        {
            b = -b;
            dot = -dot;
        }

        dot = Math.Clamp(dot, -1f, 1f);

        if (dot > 0.9995f)
            return Lerp(a, b, t);

        float theta = MathF.Acos(dot);
        float sinTheta = MathF.Sin(theta);

        float factorA = MathF.Sin((1f - t) * theta) / sinTheta;
        float factorB = MathF.Sin(t * theta) / sinTheta;

        return a * factorA + b * factorB;
    }
    public static Quaternion SLerpClamped(Quaternion a, Quaternion b, float t)
    {
        return SLerp(a, b, Math.Clamp(t, 0, 1));
    }
    public static Quaternion FromAxisAngle(Vector3D axis, Angle angle)
    {
        float halfAngle = angle.Radians / 2f;

        float sin = MathF.Sin(halfAngle);
        float cos = MathF.Cos(halfAngle);

        Vector3D normalized = axis.Normalize();

        return new Quaternion(
            normalized.X * sin,
            normalized.Y * sin,
            normalized.Z * sin,
            cos
        );
    }
    public static Quaternion FromToRotation(Vector3D from, Vector3D to)
    {
        from = from.Normalize();
        to = to.Normalize();

        float dot = Math.Clamp(from.Dot(to), -1f, 1f);

        if (dot >= 1f - 1e-6f)
            return Identity;

        if (dot <= -1f + 1e-6f)
        {
            Vector3D axis = Vector3D.Cross(
                from,
                MathF.Abs(from.X) < MathF.Abs(from.Y)
                    ? Vector3D.UnitX
                    : Vector3D.UnitY
            ).Normalize();

            return FromAxisAngle(axis, Angle.FromRadians(MathF.PI));
        }

        Vector3D cross = Vector3D.Cross(from, to);

        float s = MathF.Sqrt((1f + dot) * 2f);

        return new Quaternion(
            cross.X / s,
            cross.Y / s,
            cross.Z / s,
            s / 2f
        ).Normalize();
    }
    public static Quaternion LookRotation(Vector3D forward, Vector3D up)
    {
        forward = forward.Normalize();

        Vector3D right = Vector3D.Cross(up, forward).Normalize();

        up = Vector3D.Cross(forward, right);

        float m00 = right.X;
        float m01 = up.X;
        float m02 = forward.X;

        float m10 = right.Y;
        float m11 = up.Y;
        float m12 = forward.Y;

        float m20 = right.Z;
        float m21 = up.Z;
        float m22 = forward.Z;

        float trace = m00 + m11 + m22;

        if (trace > 0f)
        {
            float s = MathF.Sqrt(trace + 1f) * 2f;

            return new Quaternion(
                (m21 - m12) / s,
                (m02 - m20) / s,
                (m10 - m01) / s,
                s / 4f
            );
        }

        if (m00 > m11 && m00 > m22)
        {
            float s = MathF.Sqrt(1f + m00 - m11 - m22) * 2f;

            return new Quaternion(
                s / 4f,
                (m01 + m10) / s,
                (m02 + m20) / s,
                (m21 - m12) / s
            );
        }

        if (m11 > m22)
        {
            float s = MathF.Sqrt(1f + m11 - m00 - m22) * 2f;

            return new Quaternion(
                (m01 + m10) / s,
                s / 4f,
                (m12 + m21) / s,
                (m02 - m20) / s
            );
        }

        {
            float s = MathF.Sqrt(1f + m22 - m00 - m11) * 2f;

            return new Quaternion(
                (m02 + m20) / s,
                (m12 + m21) / s,
                s / 4f,
                (m10 - m01) / s
            );
        }
    }
    public static Quaternion FromEulerAngles(float x, float y, float z)
    {
        float halfX = x / 2f;
        float halfY = y / 2f;
        float halfZ = z / 2f;

        float cx = MathF.Cos(halfX);
        float sx = MathF.Sin(halfX);

        float cy = MathF.Cos(halfY);
        float sy = MathF.Sin(halfY);

        float cz = MathF.Cos(halfZ);
        float sz = MathF.Sin(halfZ);

        return new Quaternion(
            sx * cy * cz - cx * sy * sz,
            cx * sy * cz + sx * cy * sz,
            cx * cy * sz - sx * sy * cz,
            cx * cy * cz + sx * sy * sz
        );
    }
    public static Quaternion FromEulerAngles(Vector3D euler)
    {
        return FromEulerAngles(euler.X, euler.Y, euler.Z);
    }
    public static Quaternion FromEulerAngles(EulerAngles euler)
    {
        return FromEulerAngles(euler.X, euler.Y, euler.Z);
    }
    public static Quaternion operator +(Quaternion a, Quaternion b)
    {
        return new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
    }
    public static Quaternion operator -(Quaternion a, Quaternion b)
    {
        return new(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
    }
    public static Quaternion operator -(Quaternion q)
    {
        return new(-q.X, -q.Y, -q.Z, -q.W);
    }
    public static Vector3D operator *(Quaternion q, Vector3D v)
    {
        Vector3D qVector = new(q.X, q.Y, q.Z);

        Vector3D t = 2f * Vector3D.Cross(qVector, v);

        return v
            + (q.W * t)
            + Vector3D.Cross(qVector, t);
    }
    public static Quaternion operator *(Quaternion a, Quaternion b)
    {
        return new(
            a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
            a.W * b.Y - a.X * b.Z + a.Y * b.W + a.Z * b.X,
            a.W * b.Z + a.X * b.Y - a.Y * b.X + a.Z * b.W,
            a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z
        );
    }
    public static Quaternion operator *(Quaternion q, float scale)
    {
        return new(q.X * scale, q.Y * scale, q.Z * scale, q.W * scale);
    }
    public static Quaternion operator *(float scale, Quaternion q)
    {
        return q * scale;
    }
    public static Quaternion operator /(Quaternion q, float scale)
    {
        return new(q.X / scale, q.Y / scale, q.Z / scale, q.W / scale);
    }
    public static implicit operator (float x, float y, float z, float w)(Quaternion v)
    {
        return (v.X, v.Y, v.Z, v.W);
    }
    public static implicit operator Quaternion((float, float, float, float) v)
    {
        return new Quaternion(v.Item1, v.Item2, v.Item3, v.Item4);
    }
    public override int GetHashCode()
    {
        HashCode hc = new();
        hc.Add(X);
        hc.Add(Y);
        hc.Add(Z);
        hc.Add(W);
        return hc.ToHashCode();
    }
    public bool Equals(Quaternion? obj)
    {
        return this == obj;
    }
    public override bool Equals(object? obj)
    {
        if(obj is not Quaternion q)
            return false;
        return this == q;
    }
    public static bool operator ==(Quaternion? a, Quaternion? b)
    {
        if (ReferenceEquals(a, b))
            return true;
        if (a is null || b is null)
            return false;
        return a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W;
    }
    public static bool operator !=(Quaternion? a, Quaternion? b)
    {
        return !(a == b);
    }
    public Quaternion Clone()
    {
        return new(X, Y, Z, W);
    }
}
