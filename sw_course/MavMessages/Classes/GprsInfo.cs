namespace sw_course.MavMessages.Classes;

public class GprsInfo : MavMessage
{
    public GprsInfo(MAVLink.MAVLinkMessage message) : base(message)
    {
        var msgStruct = message.ToStructureGC<MAVLink.mavlink_gprs_module_info_t>();
        var boardId = msgStruct.id;
        if (BoardId == boardId) 
            return;
        
        BoardId = msgStruct.id;
        _isDifferent = true;
    }
}