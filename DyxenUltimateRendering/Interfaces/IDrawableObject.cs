using DyxenUltimateRendering.Register.Interfaces;
using DyxenUltimateRendering.Transform.Interfaces;

namespace DyxenUltimateRendering.Interfaces;

public interface IDrawableObject : ITransform2D, IID
{
    public Texture2D Draw();
}
