using sw_course.MavMessages.Classes;

namespace sw_course.MavMessages.Creators;

public class GprsInfoCreator : ICreator
{
    public MavMessage CreateMessage(MAVLink.MAVLinkMessage message) 
    {
        return new GprsInfo(message);
    }
}