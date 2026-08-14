using DyxenUltimateRendering.Transform;
using DyxenUltimateRendering.Transform.Interfaces;
using System.Text.Json.Serialization;

namespace DyxenUltimateRendering;

public class Camera : ITransform3D
{
    [JsonRequired]
    public required Transform3D Transform { get; init; }
    public readonly Viewport Viewport;
}
