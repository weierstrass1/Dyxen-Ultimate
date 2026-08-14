using DyxenUltimateRendering.Register.Interfaces;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DyxenUltimateRendering.Register;

public class IIDJsonConverter(IDManager idManager) : JsonConverter<IID>
{
    private static readonly MethodInfo mi = typeof(IDManager).GetMethod(nameof(IDManager.GetOrCreate))!;
    public override IID? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        Guid id = reader.GetGuid();
        if (!idManager.TryGetType(id, out Type? type))
            return null;

        MethodInfo gmi = mi.MakeGenericMethod(type);

        return (IID)gmi.Invoke(idManager, [id])!;
    }
    public override void Write(
        Utf8JsonWriter writer,
        IID value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ID);
    }
}
