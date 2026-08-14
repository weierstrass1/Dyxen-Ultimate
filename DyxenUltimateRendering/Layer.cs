using DyxenUltimateRendering.Interfaces;
using DyxenUltimateRendering.Register.Interfaces;
using DyxenUltimateRendering.Transform;
using DyxenUltimateRendering.Transform.Interfaces;
using System.Text.Json.Serialization;
namespace DyxenUltimateRendering;
public enum LayerType { World, UI }
public class Layer() : ITransform2D, ISize2D, IID
{
    [JsonRequired]
    public required Guid ID { get; init; }
    [JsonRequired]
    public required Transform2D Transform { get; init; }
    public int Width { get; }
    public int Height { get; }
    [JsonInclude]
    public LayerType Type { get; set; }
    [JsonRequired]
    public PriorityContainer<IDrawableObject> DrawableObjects { get; init; } = new();
}
