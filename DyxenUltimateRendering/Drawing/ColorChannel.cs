namespace DyxenUltimateRendering.Drawing;
public enum CommonColorChannels
{
    Red,
    Green,
    Blue,
    Hue,
    Saturation,
    Lightness,
    Value,
    Cyan,
    Magenta,
    Yellow,
    Black,
    Alpha
}
public readonly struct ColorChannel<TID> where TID : Enum
{
    public TID ID { get; }
    public int BitDepth { get; }
    public int BitAlign { get; }
    public int MaxValue { get; }
    public float FMaxValue { get; }
    private readonly bool _customMaxValue;
    private readonly int _rawMaxValue;
    public ColorChannel(TID id, int bitDepth, int bitAlign)
    {
        ID = id;
        BitDepth = bitDepth;
        BitAlign = bitAlign;
        MaxValue = (1 << bitDepth) - 1;
        FMaxValue = MaxValue;
        _rawMaxValue = MaxValue;
        _customMaxValue = false;
    }
    public ColorChannel(TID id, int bitDepth, int bitAlign, int maxValue)
    {
        ID = id;
        BitDepth = bitDepth;
        BitAlign = bitAlign;
        MaxValue = maxValue;
        FMaxValue = maxValue;
        _rawMaxValue = (1 << bitDepth) - 1;
        _customMaxValue = true;
    }
    public byte GetRawValue(int value)
    {
        int v = (value >> BitAlign) & _rawMaxValue;
        return (byte)(_customMaxValue ?
            Math.Clamp(v, 0, MaxValue) :
            v);
    }
    public byte ToWholeByte(int value)
    {
        if(BitDepth == 8 && !_customMaxValue)
            return GetRawValue(value);
        return (byte)(255 * GetRawValue(value) / MaxValue);
    }
    public float ToNormalizedFloat(int value)
    {
        return GetRawValue(value) / FMaxValue;
    }
}
