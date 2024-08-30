namespace OperationContext.Core.Tests;

public class OperationContextLogScopeTests
{
    [Test]
    public void WrapsProvidedDictionary()
    {
        var source = new Dictionary<string, string>()
        {
            { "Key1", "Value1" },
            { "Key2", "Value2" },
        };
        var target = new OperationContextLogScope(source);

        Assert.That(target, Has.Exactly(source.Count).Items);
    }

    [Test]
    public void Index_ReturnsItemsFromSourceDictionary()
    {
        var source = new Dictionary<string, string>()
        {
            { "Key1", "Value1" },
            { "Key2", "Value2" },
        };
        var target = new OperationContextLogScope(source);

        Assert.That(target[0], Is.EqualTo(KeyValuePair.Create("Key1", (object)"Value1")));
        Assert.That(target[1], Is.EqualTo(KeyValuePair.Create("Key2", (object)"Value2")));
    }

    [Test]
    public void GetEnumerator_ReturnsItemsFromSourceDictionary()
    {
        var source = new Dictionary<string, string>()
        {
            { "Key1", "Value1" },
            { "Key2", "Value2" },
        };
        var target = new OperationContextLogScope(source);
        var enumerator = target.GetEnumerator();

        enumerator.MoveNext();
        Assert.That(enumerator.Current, Is.EqualTo(KeyValuePair.Create("Key1", (object)"Value1")));

        enumerator.MoveNext();
        Assert.That(enumerator.Current, Is.EqualTo(KeyValuePair.Create("Key2", (object)"Value2")));
    }

    [Test]
    public void ToString_SerializesItemsToString()
    {
        var source = new Dictionary<string, string>()
        {
            { "Key1", "Value1" },
            { "Key2", "Value2" },
        };
        var target = new OperationContextLogScope(source);

        Assert.That(target.ToString(), Is.EqualTo("Key1: Value1, Key2: Value2"));
    }
}
