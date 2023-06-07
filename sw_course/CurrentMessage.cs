using System.Globalization;
using sw_course.Enums;
using sw_course.MavMessages;
using sw_course.MavMessages.Creators;

namespace sw_course;

public static class CurrentMessage
{
    public static MavMessage GetMessage(MAVLink.MAVLinkMessage message)
    {
        return message.msgid switch 
        {
            (uint) MAVLink.MAVLINK_MSG_ID.GPRS_MODULE_INFO => new GprsInfoCreator().CreateMessage(message),
            (uint) MAVLink.MAVLINK_MSG_ID.GPRS_MODULE_STATUS => new GprsStatusCreator().CreateMessage(message),
            (uint) MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT => new GlobalPositionCreator().CreateMessage(message),
            (uint) MAVLink.MAVLINK_MSG_ID.GPS_RAW_INT => new GpsRawCreator().CreateMessage(message),
            (uint) MAVLink.MAVLINK_MSG_ID.GPS2_RAW => new Gps2RawCreator().CreateMessage(message),
            (uint) MAVLink.MAVLINK_MSG_ID.BATTERY_STATUS => new BatteryStatusCreator().CreateMessage(message),
            (uint) MAVLink.MAVLINK_MSG_ID.SYS_STATUS => new SysStatusCreator().CreateMessage(message),
            (uint) MAVLink.MAVLINK_MSG_ID.HEARTBEAT => new HeartbeatCreator().CreateMessage(message),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}