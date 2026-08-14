using DyxenUltimateRendering.Drawing.Interfaces;
namespace DyxenUltimateRendering.Drawing.ColorFormats;
public class RGB888 : IColorFormat<CommonColorChannels>
{
    public ColorChannel<CommonColorChannels> R = new(CommonColorChannels.Red, 8, 16);
    public ColorChannel<CommonColorChannels> G = new(CommonColorChannels.Green, 8, 8);
    public ColorChannel<CommonColorChannels> B = new(CommonColorChannels.Blue, 8, 0);
    public IEnumerable<ColorChannel<CommonColorChannels>> Channels 
    { 
        get 
        {
            yield return R;
            yield return G;
            yield return B;
        }
    }
}
