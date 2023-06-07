namespace sw_course.MavMessages.Classes;

public class GprsStatus : MavMessage
{
    public GprsStatus(MAVLink.MAVLinkMessage message) : base(message)
    {
        var msgStruct = message.ToStructureGC<MAVLink.mavlink_gprs_module_status_t>();
        var boardId = msgStruct.id;
        var gprsSignalLevel = msgStruct.signal_level;
        if (boardId == BoardId && GprsSignalLevel == gprsSignalLevel) 
            return;
        
        BoardId = boardId;
        GprsSignalLevel = gprsSignalLevel;
        _isDifferent = true;
    }
}