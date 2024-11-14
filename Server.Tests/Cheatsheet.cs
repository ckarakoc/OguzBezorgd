namespace Server.Tests;

using System.Reflection;
using Xunit.Sdk;

public class Cheatsheet
{
    // STRINGS
    private const string ExpectedString = "";
    private const string ActualString = "";
    private const string StringToCheck = "HelloWorld";

    // COLLECTIONS
    private const string ExpectedThing = "Hello";
    private static readonly List<string> Collection = ["Hello", "World", "Test"];
    private const string ThingToCheck = "Hello";
    private static readonly List<string> EmptyCollection = [];
    private static readonly List<string> NonEmptyCollection = ["Test1", "Test2", "Test3"];

    // NUMBERS
    private const int LowRange = 1;
    private const int HighRange = 10;
    private const int ThingToCheckInRange = 5;

    // TYPES
    private static readonly object Obj1 = new();
    private static readonly object Obj2 = Obj1;
    private static readonly List<string> Thing = [];

    // EXCEPTION TESTS
    private static readonly SutClass Sut = new();

    [Fact]
    public void Test()
    {
        // STRINGS
        Assert.Equal(ExpectedString, ActualString);
        Assert.StartsWith(ExpectedString, StringToCheck);
        Assert.EndsWith(ExpectedString, StringToCheck);

        // Optional parameters
        Assert.Equal(ExpectedString, ActualString, ignoreCase: true);
        Assert.StartsWith(ExpectedString, StringToCheck, StringComparison.OrdinalIgnoreCase);

        // COLLECTIONS
        Assert.Contains(ExpectedThing, Collection);
        Assert.Contains(Collection, item => item.Contains(ThingToCheck));
        Assert.DoesNotContain(ExpectedThing, EmptyCollection);
        Assert.Empty(EmptyCollection);
        Assert.All(NonEmptyCollection, item => Assert.False(string.IsNullOrWhiteSpace(item)));

        // NUMBERS
        Assert.InRange(ThingToCheckInRange, LowRange, HighRange);

        // EXCEPTIONS
        Assert.Throws<InvalidOperationException>(() => Sut.Method());

        // TYPES
        Assert.IsType<List<string>>(Thing);
        Assert.IsAssignableFrom<IEnumerable<string>>(Thing);
        Assert.Same(Obj1, Obj2);
        Assert.NotSame(Obj1, new object());
    }
}

// Example Sut class to throw exceptions for tests
public class SutClass
{
    public void Method() => throw new InvalidOperationException();
}

// COOL XUNIT STUFF

// Inherit from the DataAttribute from xunit.sdk
public class CustomData : DataAttribute
{
    // Needs a method that returns an IEnumerable<object[]>
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        yield return new object[] {"data1", 0, DateTime.Now};
        yield return new object[] {"data4", 1, DateTime.MaxValue};
    }
}

// Create the fixture to share
public class _TestSetup : IDisposable
{
    // Some stuff here
    public void Dispose()
    {
    }
}

// xUnit’s fixture mechanism ensures that the _TestSetup is disposed of after all tests in the class have completed.
public class _TestClass : IClassFixture<_TestSetup>
{
    public _TestClass(_TestSetup setup)
    {
        // Initialise setup to share across test methods
    }

    [Theory]
    [CustomData]
    public void _TestMethod(string param1, int param2, DateTime param3)
    {
        // Test using the 3 params
        Assert.True(true);
    }
}

// Create collection to share across test classes
[CollectionDefinition("Name of Collection")]
public class _TestCollection : ICollectionFixture<_TestSetup>
{
}

[Collection("Name of Collection")]
public class _TestClass2
{
    public _TestClass2(_TestSetup setup)
    {
        // Initialise setup
    }

    [Fact]
    public void Test()
    {
        Assert.True(true);
    }
}