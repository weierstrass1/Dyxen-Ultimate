using DyxenUltimateRendering.Register.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace DyxenUltimateRendering.Register;

public class IDManager
{
    [JsonRequired]
    public required Dictionary<Guid, string> Types { get; init; }
    public readonly Dictionary<Guid, IID> _ids = [];
    public Guid GenerateID(IID element)
    {
        Guid id;

        do
        {
            id = Guid.NewGuid();
        }
        while (!_ids.TryAdd(id, element));

        Types.Add(id, element.GetType().AssemblyQualifiedName!);

        return id;
    }
    public bool Register(Guid id, IID element)
    {
        if (!_ids.TryAdd(id, element))
            return false;
        Types.Add(id, element.GetType().AssemblyQualifiedName!);
        return true;
    }
    public bool Contains(Guid id)
    {
        return _ids.ContainsKey(id);
    }
    public bool TryGet(Guid id, out IID? element)
    {
        return _ids.TryGetValue(id, out element);
    }
    public bool TryGetType(Guid id, [NotNullWhen(true)] out Type? type)
    {
        if (!Types.TryGetValue(id, out string name))
        {
            type = null;
            return false;
        }
        type = Type.GetType(name);
        return type != null;
    }
    public IID GetOrCreate<T>(Guid id) where T : IID<T>
    {
        if (_ids.TryGetValue(id, out IID? element))
            return element;
        IID instance = T.CreateInstance(id);
        _ids.Add(id, instance);
        Types.Add(id, instance.GetType().AssemblyQualifiedName!);
        return instance;
    }
}
