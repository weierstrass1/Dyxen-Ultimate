using DyxenUltimateRendering.Register.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace DyxenUltimateRendering;

public class PriorityContainer<T> where T : IID
{
    public IEnumerable<T> Elements => _elements.Values.SelectMany(e => e.Values);
    private readonly SortedDictionary<int, Dictionary<Guid, T>> _elements = [];
    public bool TryAdd(int priority, T element)
    {
        if (!Remove(element))
            return false;

        return TryAdd(priority, element);
    }
    public bool TryGetPriority(T e, [NotNullWhen(true)] out int? priority)
    {
        foreach (var item in _elements)
        {
            if (item.Value.ContainsKey(e.ID))
            {
                priority = item.Key;
                return true;
            }
        }
        priority = null;
        return false;
    }
    public bool Remove(T element)
    {
        int? p = null;
        foreach (var item in _elements)
        {
            if (item.Value.Remove(element.ID))
            {
                p = item.Key;
                break;
            }
        }
        if (p == null)
            return false;

        if (_elements[p.Value].Count == 0)
            _elements.Remove(p.Value);
        return true;
    }
    public bool SetPriority(int priority, T element)
    {
        if (!Remove(element))
            return false;
        TryAdd(priority, element);
        return true;
    }
}
