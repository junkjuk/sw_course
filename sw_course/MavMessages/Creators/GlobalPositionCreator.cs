using sw_course.MavMessages.Classes;

namespace sw_course.MavMessages.Creators;

public class GlobalPositionCreator : ICreator
{
    public MavMessage CreateMessage(MAVLink.MAVLinkMessage message)
    {
        return new GlobalPosition(message);
    }
}