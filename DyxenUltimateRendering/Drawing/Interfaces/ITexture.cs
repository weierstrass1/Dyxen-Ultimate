using DyxenUltimateRendering.Interfaces;
namespace DyxenUltimateRendering.Drawing.Interfaces;

public interface ITexture<TColorFormat> : ISize2D 
    where TColorFormat : IColorFormat
{

}
