namespace DyxenUltimateRendering.Drawing;
public readonly struct Color
{
    public uint Value { get; }
    public byte R => (byte)(Value >> 24);
    public byte G => (byte)(Value >> 16);
    public byte B => (byte)(Value >> 8);
    public byte A => (byte)Value;
    public float NormalizedR => R / 255f;
    public float NormalizedG => G / 255f;
    public float NormalizedB => B / 255f;
    public float NormalizedA => A / 255f;
    public float Hue
    {
        get
        {
            float r = NormalizedR;
            float g = NormalizedG;
            float b = NormalizedB;

            float max = MathF.Max(r, MathF.Max(g, b));
            float min = MathF.Min(r, MathF.Min(g, b));
            float delta = max - min;

            if (delta == 0)
                return 0f;

            float hue;

            if (max == r)
            {
                hue = 60f * ((g - b) / delta);

                if (hue < 0)
                    hue += 360f;
                return hue;
            }

            return max == g ?
                60f * ((b - r) / delta + 2f):
                60f * ((r - g) / delta + 4f);
        }
    }
    public float HSVSaturation
    {
        get
        {
            float max = MathF.Max(
                NormalizedR,
                MathF.Max(NormalizedG, NormalizedB));

            if (max == 0f)
                return 0f;

            float min = MathF.Min(
                NormalizedR,
                MathF.Min(NormalizedG, NormalizedB));

            return (max - min) / max;
        }
    }
    public float HSVValue => MathF.Max(NormalizedR, MathF.Max(NormalizedG, NormalizedB));
    public float HSLSaturation
    {
        get
        {
            float max = MathF.Max(
                NormalizedR,
                MathF.Max(NormalizedG, NormalizedB));

            float min = MathF.Min(
                NormalizedR,
                MathF.Min(NormalizedG, NormalizedB));

            float delta = max - min;

            if (delta == 0f)
                return 0f;

            float lightness = (max + min) * 0.5f;

            return delta / (1f - MathF.Abs(2f * lightness - 1f));
        }
    }
    public float Lightness
    {
        get
        {
            float max = MathF.Max(
                NormalizedR,
                MathF.Max(NormalizedG, NormalizedB));

            float min = MathF.Min(
                NormalizedR,
                MathF.Min(NormalizedG, NormalizedB));

            return (max + min) * 0.5f;
        }
    }
    public float RGBMean => (NormalizedR + NormalizedG + NormalizedB) / 3f;
    public float Luminance => (0.2126f * NormalizedR) + (0.7152f * NormalizedG) + (0.0722f * NormalizedB);
    public (byte R, byte G, byte B, byte A) ToRGBATuple()
    {
        return (R, G, B, A); 
    }
    public (float R, float G, float B, float A) ToRGBAFloatTuple()
    {
        return (NormalizedR, NormalizedG, NormalizedB, NormalizedA);
    }
    public (float H, float S, float V) ToHSVTuple()
    {
        return (Hue, HSVSaturation, HSVValue);
    }
    public (float H, float S, float L) ToHSLTuple()
    {
        return (Hue, HSLSaturation, Lightness);
    }
}
