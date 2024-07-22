namespace OperationContext.Core;

public class DefaultOperationContext : IOperationContext
{
    private static readonly AsyncLocal<ItemsHolder> asyncLocal = new();

    public IDictionary<string, string>? Items
    {
        get { return asyncLocal.Value?.Items; }
        set
        {
            var holder = asyncLocal.Value;
            if (holder is not null)
            {
                holder.Items = null;
            }

            if (value != null)
            {
                asyncLocal.Value = new ItemsHolder { Items = value };
            }
        }
    }

    private class ItemsHolder
    {
        public IDictionary<string, string>? Items { get; set; }
    }
}
