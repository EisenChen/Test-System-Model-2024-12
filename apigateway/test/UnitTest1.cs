namespace test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();        
    }
    [Test]
    public void Test2(){
        var b = 1;
        Assert.That(b, Is.EqualTo(1));
        // Assert.That(b, Is.EqualTo(4));
    }
}