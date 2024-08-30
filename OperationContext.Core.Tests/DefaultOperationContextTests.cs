namespace OperationContext.Core.Tests;

public class DefaultOperationContextTests
{
    [Test]
    public void StoresValues()
    {
        var target = new DefaultOperationContext { Items = new Dictionary<string, string>() };

        target.Items.Add("Test", "Passed");
        target.Items.Add("AnotherTest", "Passed again");

        Assert.That(target.Items, Contains.Key("Test").WithValue("Passed"));
        Assert.That(target.Items, Contains.Key("AnotherTest").WithValue("Passed again"));
        Assert.That(target.Items, Has.Exactly(2).Items);
    }

    [Test]
    public Task UsesSeparateStorageForEachAsyncOperation()
    {
        var target = new DefaultOperationContext();

        var task1 = Task.Run(() =>
        {
            target.Items = new Dictionary<string, string>();
            target.Items.Add("Test", "Passed");

            Assert.That(target.Items, Contains.Key("Test").WithValue("Passed"));
            Assert.That(target.Items, Has.One.Items);
        });
        var task2 = Task.Run(() =>
        {
            target.Items = new Dictionary<string, string>();
            target.Items.Add("AnotherTest", "Passed again");

            Assert.That(target.Items, Contains.Key("AnotherTest").WithValue("Passed again"));
            Assert.That(target.Items, Has.One.Items);
        });
        var task3 = Task.Delay(100);

        return Task.WhenAll(task1, task2, task3);
    }

    [Test]
    public async Task CleansOperationContextUponCompletion()
    {
        var target = new DefaultOperationContext();

        var task1 = Task.Run(() =>
        {
            target.Items = new Dictionary<string, string>();
            target.Items.Add("Test", "Passed");
        });
        var task2 = Task.Delay(100);

        await Task.WhenAll(task1, task2);

        Assert.That(target.Items, Is.Null);
    }
}
