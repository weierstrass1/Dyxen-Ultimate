using DyxenUltimateRendering.Register;
using DyxenUltimateRendering.Register.Interfaces;
using DyxenUltimateRendering.Transform.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace DyxenUltimateRendering.Transform;

public class Transform3D : IPosition<Vector3D>, IRotate3D, IScale<Vector3D>, IID<Transform3D>
{
    [JsonRequired]
    public required Guid ID { get; init; }
    [JsonIgnore]
    private Transform3D? _parent { get; set; } = null;
    [JsonInclude]
    public Transform3D? Parent
    {
        get => _parent;
        set
        {
            Vector3D pos = Position;
            Quaternion rot = Rotation;
            Vector3D scale = Scale;
            if (_parent is not null)
            {
                _parent.PositionChanged -= MakePositionDirty;
                _parent.RotationChanged -= MakeRotationDirty;
                _parent.ScaleChanged -= MakeScaleDirty;
            }
            _parent = value;
            if (_parent is not null)
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
    private Vector3D _localPosition { get; set; } = Vector3D.Zero.Clone();
    [JsonIgnore]
    private Quaternion _localRotation { get; set; } = Quaternion.Identity.Clone();
    [JsonIgnore]
    private Vector3D _localScale { get; set; } = Vector3D.One.Clone();
    [JsonIgnore]
    private Vector3D? _position = null;
    [JsonIgnore]
    private Quaternion? _rotation = null;
    [JsonIgnore]
    private Vector3D? _scale = null;
    [JsonInclude]
    public Vector3D LocalPosition
    {
        get => _localPosition;
        set
        {
            Vector3D oldPos = _localPosition;
            _localPosition = value;
            MakePositionDirty(this, oldPos);
        }
    }
    [JsonInclude]
    public Quaternion LocalRotation
    {
        get => _localRotation;
        set
        {
            Vector3D oldPos = _localPosition;
            Quaternion oldrot = _localRotation;
            _localRotation = value;
            MakeRotationDirty(this, oldrot);
            MakePositionDirty(this, oldPos);
        }
    }
    [JsonInclude]
    public Vector3D LocalScale
    {
        get => _localScale;
        set
        {
            Vector3D oldPos = _localPosition;
            Vector3D oldScale = _localScale;
            _localScale = value;
            MakeScaleDirty(this, oldScale);
            MakePositionDirty(this, oldPos);
        }
    }
    public Vector3D Position
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
    public Quaternion Rotation
    {
        get => _rotation is not null ? 
                    _rotation :
                    Parent == null ?
                            LocalRotation :
                            Parent.Rotation * LocalRotation;
        set
        {
            LocalRotation = Parent == null ?
                    value :
                    Parent.Rotation.Inverse() * value;
            _rotation = value;
        }
    }
    public Vector3D Scale
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
    public Vector3D Right => Rotation.Right;
    public Vector3D Up => Rotation.Up;
    public Vector3D Forward => Rotation.Forward;
    public event Action<Transform3D, Vector3D>? PositionChanged;
    public event Action<Transform3D, Quaternion>? RotationChanged;
    public event Action<Transform3D, Vector3D>? ScaleChanged;
    public Transform3D()
    {
    }
    [SetsRequiredMembers]
    public Transform3D(IDManager idManager)
    {
        ID = idManager.GenerateID(this);
    }
    public void Translate(Vector3D translation)
    {
        Position += translation;
    }
    public void Rotate(Quaternion rotation)
    {
        Rotation = (Rotation * rotation).Normalize();
    }
    public void ScaleBy(Vector3D scale)
    {
        Scale *= scale;
    }
    public void LookAt(Vector3D target)
    {
        Vector3D direction = target - Position;

        if (direction.MagnitudeSquared == 0)
            return;

        Rotation = Quaternion.LookRotation(
            direction,
            Vector3D.UnitY
        );
    }
    public Vector3D TransformPoint(Vector3D local)
    {
        local *= LocalScale;
        local = LocalRotation * local;
        local += LocalPosition;

        return Parent == null ?
                    local :
                    Parent.TransformPoint(local);
    }
    public Vector3D InverseTransformPoint(Vector3D world)
    {
        if (Parent != null)
            world = Parent.InverseTransformPoint(world);

        Vector3D local = world - LocalPosition;

        local = LocalRotation.Inverse() * local;
        local /= LocalScale;

        return local;
    }
    private void MakePositionDirty(Transform3D sender, Vector3D oldPosition)
    {
        _position = null;
        PositionChanged?.Invoke(this, oldPosition);
    }
    private void MakeRotationDirty(Transform3D sender, Quaternion oldRotation)
    {
        _rotation = null;
        RotationChanged?.Invoke(this, oldRotation);
    }
    private void MakeScaleDirty(Transform3D sender, Vector3D oldScale)
    {
        _scale = null;
        ScaleChanged?.Invoke(this, oldScale);
    }
    public static Transform3D CreateInstance(IDManager idManager)
    {
        return new Transform3D(idManager);
    }
    static Transform3D IID<Transform3D>.CreateInstance(Guid id)
    {
        return new Transform3D()
        {
            ID = id
        };
    }
}
