using System.IO;
using System.Linq;
using NUnit.Framework;
using sw_course;

namespace sw_course_test;

public class ReaderFromFileTest
{
    [Test]
    public void Test1()
    {
        var file = Path.Combine(Directory.GetCurrentDirectory(), "testLogEmpty.tlog");
        using var reader = new ReaderFromFile(file);
        Assert.AreEqual(reader.GetMessagesInCsvFormat().Count(), 1);
    }

    [Test]
    public void Test2()
    {
        var file = Path.Combine(Directory.GetCurrentDirectory(), "testLog.tlog");
        using var reader = new ReaderFromFile(file);
        Assert.AreEqual(reader.GetMessagesInCsvFormat().Count(), 663);
    }

    [Test]
    public void Test3()
    {
        Assert.Throws<FileNotFoundException>(() => _ = new ReaderFromFile("blablabla"));
    }
}