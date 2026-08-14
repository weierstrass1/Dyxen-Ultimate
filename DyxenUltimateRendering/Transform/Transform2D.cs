using DyxenUltimateRendering.Register;
using DyxenUltimateRendering.Register.Interfaces;
using DyxenUltimateRendering.Transform.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace DyxenUltimateRendering.Transform;

public class Transform2D : IPosition<Vector2D>, IRotate2D, IScale<Vector2D>, IID<Transform2D>
{
    [JsonRequired]
    public required Guid ID { get; init; }
    [JsonIgnore]
    private Transform2D? _parent { get; set; } = null;
    [JsonInclude]
    public Transform2D? Parent 
    {
        get => _parent;
        set
        {
            Vector2D pos = Position;
            Angle rot = Rotation;
            Vector2D scale = Scale;
            if (_parent is not null)
            {
                _parent.PositionChanged -= MakePositionDirty;
                _parent.RotationChanged -= MakeRotationDirty;
                _parent.ScaleChanged -= MakeScaleDirty;
            }
            _parent = value;
            if( _parent is not null)
            {
                _parent.PositionChanged += MakePositionDirty;
                _parent.RotationChanged += MakeRotationDirty;
                _parent.ScaleChanged += MakeScaleDirty;
            }
            Position = pos;
            Rotation = rot;
            Scale = scale;
        }
    }
    [JsonIgnore]
    private Vector2D _localPosition { get; set; } = Vector2D.Zero.Clone();
    [JsonIgnore]
    private Angle _localRotation { get; set; } = 0;
    [JsonIgnore]
    private Vector2D _localScale { get; set; } = Vector2D.One.Clone();
    [JsonIgnore]
    private Vector2D? _position = null;
    [JsonIgnore]
    private Angle? _rotation = null;
    [JsonIgnore]
    private Vector2D? _scale = null;
    [JsonInclude]
    public Vector2D LocalPosition
    {
        get => _localPosition;
        set
        {
            Vector2D oldPos = _localPosition;
            _localPosition = value;
            _position = null;
            PositionChanged?.Invoke(this, oldPos);
        }
    }
    [JsonInclude]
    public Angle LocalRotation
    {
        get => _localRotation;
        set
        {
            Angle oldrot = _localRotation;
            _localRotation = value;
            _rotation = null;
            _position = null;
            RotationChanged?.Invoke(this, oldrot);
        }
    }
    [JsonInclude]
    public Vector2D LocalScale
    {
        get => _localScale;
        set
        {
            Vector2D oldScale = _localScale;
            _localScale = value;
            _scale = null;
            _position = null;
            ScaleChanged?.Invoke(this, oldScale);
        }
    }
    [JsonIgnore]
    public Vector2D Position
    {
        get => _position is not null ?
                    _position :
                    Parent == null ?
                            LocalPosition :
                            Parent.TransformPoint(LocalPosition);

        set
        {
            LocalPosition = Parent == null ?
                    value :
                    Parent.InverseTransformPoint(value);
            _position = value;
        }
    }
    [JsonIgnore]
    public Angle Rotation
    {
        get => _rotation is not null ?
                    _rotation :
                    Parent == null ?
                            LocalRotation :
                            Parent.Rotation + LocalRotation;

        set
        {
            LocalRotation = Parent == null ?
                    value :
                    value - Parent.Rotation;
            _rotation = value;
        }
    }
    [JsonIgnore]
    public Vector2D Scale
    {
        get => _scale is not null ? 
                    _scale :
                    Parent == null ?
                            LocalScale :
                            Parent.Scale * LocalScale;

        set
        {
            LocalScale = Parent == null ?
                    value :
                    value / Parent.Scale;
            _scale = value;
        }
    }
    [JsonIgnore]
    public Vector2D Right => new(Rotation.Cos, Rotation.Sin);
    [JsonIgnore]
    public Vector2D Up => new(-Rotation.Sin, Rotation.Cos);
    public event Action<Transform2D, Vector2D>? PositionChanged;
    public event Action<Transform2D, Angle>? RotationChanged;
    public event Action<Transform2D, Vector2D>? ScaleChanged;
    public Transform2D()
    {
    }
    [SetsRequiredMembers]
    public Transform2D(IDManager idManager)
    {
        ID = idManager.GenerateID(this);
    }
    public void Translate(Vector2D translation)
    {
        Position += translation;
    }
    public void Rotate(Angle angle)
    {
        Rotation += angle;
    }
    public void ScaleBy(Vector2D scale)
    {
        Scale *= scale;
    }
    public void LookAt(Vector2D target)
    {
        Vector2D direction = target - Position;

        if (direction.MagnitudeSquared == 0)
            return;

        Rotation = MathF.Atan2(direction.Y, direction.X);
    }
    public Vector2D TransformPoint(Vector2D local)
    {
        local *= LocalScale;

        float cos = LocalRotation.Cos;
        float sin = LocalRotation.Sin;

        Vector2D transformed = new(
            local.X * cos - local.Y * sin,
            local.X * sin + local.Y * cos
        );

        transformed += LocalPosition;

        return Parent == null ?
                    transformed :
                    Parent.TransformPoint(transformed);
    }
    public Vector2D InverseTransformPoint(Vector2D world)
    {
        if (Parent != null)
            world = Parent.InverseTransformPoint(world);

        Vector2D local = world - LocalPosition;

        float cos = LocalRotation.Cos;
        float sin = LocalRotation.Sin;

        local = new(
            local.X * cos + local.Y * sin,
            -local.X * sin + local.Y * cos
        );

        local /= LocalScale;

        return local;
    }
    private void MakePositionDirty(Transform2D sender, Vector2D oldPosition)
    {
        _position = null;
    }
    private void MakeRotationDirty(Transform2D sender, Angle oldRotation)
    {
        _rotation = null;
        _position = null;
    }
    private void MakeScaleDirty(Transform2D sender, Vector2D oldScale)
    {
        _scale = null;
        _position = null;
    }
    public static Transform2D CreateInstance(IDManager idManager)
    {
        return new Transform2D(idManager);
    }
    static Transform2D IID<Transform2D>.CreateInstance(Guid id)
    {
        return new Transform2D()
        {
            ID = id
        };
    }
}
