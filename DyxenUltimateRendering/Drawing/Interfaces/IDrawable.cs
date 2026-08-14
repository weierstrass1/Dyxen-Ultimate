using DyxenUltimateRendering.Register.Interfaces;
using DyxenUltimateRendering.Transform.Interfaces;
namespace DyxenUltimateRendering.Drawing.Interfaces;
public interface IDrawable<TTexture, TColorFormat, TRenderPipeline> : ITransform2D, IID 
    where TTexture : ITexture<TColorFormat>
    where TRenderPipeline : IRenderPipeline
    where TColorFormat : IColorFormat
{
    public IEnumerable<TRenderPipeline> Pipelines { get; }
    public TTexture BuildBaseTexture();
}
