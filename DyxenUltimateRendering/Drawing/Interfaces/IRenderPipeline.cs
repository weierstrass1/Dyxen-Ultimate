namespace DyxenUltimateRendering.Drawing.Interfaces;
public interface IRenderPipeline
{
    public IEnumerable<IShader2D> Shaders { get; }
}
public interface IRenderPipeline<TShader> : IRenderPipeline
    where TShader : IShader2D
{
    public new IEnumerable<TShader> Shaders { get; }
}
