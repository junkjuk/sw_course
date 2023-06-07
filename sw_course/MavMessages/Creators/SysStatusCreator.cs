using sw_course.MavMessages.Classes;

namespace sw_course.MavMessages.Creators;

public class SysStatusCreator : ICreator
{
    public MavMessage CreateMessage(MAVLink.MAVLinkMessage message)
    {
        return new SysStatus(message);
    }
}