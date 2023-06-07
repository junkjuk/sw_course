using sw_course.MavMessages.Classes;

namespace sw_course.MavMessages.Creators;

public class Gps2RawCreator : ICreator
{
    public MavMessage CreateMessage(MAVLink.MAVLinkMessage message)
    {
        return new Gps2Raw(message);
    }
}