using System.Linq;
using NUnit.Framework;
using sw_course;

namespace sw_course_test;

public class CurrentMessageTest
{
    private TLogReader _tLogReader;
    private byte[] Heartbyte;
    private byte[] GlobalPos;
    private byte[] Gps2Row;
    
    [SetUp]
    public void Setup()
    {
        _tLogReader = new TLogReader();
        Heartbyte = Utils.GetHeartbeatMessage();
        GlobalPos = Utils.GetGlobalPosMessage();
        Gps2Row = Utils.GetGps2RowMessage();
    }

    [Test]
    public void Test1()
    {
        var stream = Utils.GetStreamMessage(Heartbyte);
        var messages = _tLogReader.ReadFromLogFile(stream);
        foreach (var message in messages)
        {
            var packet = new CurrentMessage(message);
        }

        Assert.AreEqual(CurrentMessage.Latitude, 0);
    }

    [Test]
    public void Test2()
    {
        var stream = Utils.GetStreamMessage(GlobalPos);
        var messages = _tLogReader.ReadFromLogFile(stream);
        foreach (var message in messages)
        {
            var packet = new CurrentMessage(message);
        }
        
        Assert.True(CurrentMessage.Latitude - 1 < 0.1);
    }
    
    [Test]
    public void Test3()
    {
        var stream = Utils.GetStreamMessage(Gps2Row);
        var messages = _tLogReader.ReadFromLogFile(stream);
        foreach (var message in messages)
        {
            var packet = new CurrentMessage(message);
        }
        Assert.True(CurrentMessage.Latitude - 2 < 0.1);
    }
}