namespace sw_course.MavMessages;

public interface ICreator
{
    public MavMessage CreateMessage(MAVLink.MAVLinkMessage message);
}