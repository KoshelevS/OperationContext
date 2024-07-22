using System.Collections;

namespace OperationContext.Core;

public class OperationContextLogScope(IDictionary<string, string> items)
    : IReadOnlyList<KeyValuePair<string, object>>
{
    public KeyValuePair<string, object> this[int index]
    {
        get
        {
            var item = items.ElementAt(index);
            return KeyValuePair.Create(item.Key, (object)item.Value);
        }
    }

    public int Count => items.Count;

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        return items.Select(x => KeyValuePair.Create(x.Key, (object)x.Value)).GetEnumerator();
    }

    public override string ToString()
    {
        return string.Join(", ", items.Select(item => $"{item.Key}: {item.Value}"));
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
