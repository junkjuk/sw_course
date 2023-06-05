using System.IO;
using System.Linq;
using NUnit.Framework;
using sw_course;

namespace sw_course_test;

public class TLogReaderTest
{
    private TLogReader _tLogReader;
    
    [SetUp]
    public void Setup()
    {
        _tLogReader = new TLogReader();
    }

    [Test]
    public void Test1()
    {
        var stream = Utils.GetStreamMessage(Utils.GetEmptyMessage());
        var messages = _tLogReader.ReadFromLogFile(stream);
        Assert.AreEqual(messages.Count(), 0);
    }

    [Test]
    public void Test2()
    {
        var stream = Utils.GetStreamMessage(Utils.GetHeartbeatMessage());
        var messages = _tLogReader.ReadFromLogFile(stream);
        Assert.AreEqual(messages.Count(), 1);
    }
    
    [Test]
    public void Test3()
    {
        var stream = Utils.GetStreamMessage(
            Utils.GetHeartbeatMessage()
                .Concat(Utils.GetEmptyMessage())
                .Concat(Utils.GetHeartbeatMessage()).ToArray());
        var messages = _tLogReader.ReadFromLogFile(stream);
        Assert.AreEqual(messages.Count(), 2);
    }
}