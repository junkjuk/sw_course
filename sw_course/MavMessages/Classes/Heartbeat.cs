using sw_course.Enums;

namespace sw_course.MavMessages.Classes;

public class Heartbeat : MavMessage
{
    public Heartbeat(MAVLink.MAVLinkMessage message) : base(message)
    {
        var hb = message.ToStructureGC<MAVLink.mavlink_heartbeat_t>();
        var apName = (MAVLink.MAV_AUTOPILOT) hb.autopilot;
        var apType = (MAVLink.MAV_TYPE) hb.type;
        var customMode = hb.custom_mode;

        if (apName == _apName &&
            apType == _apType &&
            customMode == _custom_mode)
        {
            _isDifferent = true;
            return;
        }

        DroneFirmware = GetDroneFirmware(apName, apType);

        if (hb.type == (byte) MAVLink.MAV_TYPE.GCS)
            return;

        var modesList = MavCommon.GetModesList(DroneFirmware);
        if (modesList == null)
            return;

        _custom_mode = hb.custom_mode;
        foreach (var pair in modesList.Where(pair => pair.Key == hb.custom_mode))
        {
            Mode = pair.Value;
            break;
        }
    }

    private static DroneFirmware GetDroneFirmware(MAVLink.MAV_AUTOPILOT apName, MAVLink.MAV_TYPE apType)
        => apName switch
            {
                MAVLink.MAV_AUTOPILOT.ARDUPILOTMEGA => apType switch
                {
                    MAVLink.MAV_TYPE.FIXED_WING
                        or MAVLink.MAV_TYPE.FLAPPING_WING
                        or >= MAVLink.MAV_TYPE.VTOL_TAILSITTER_DUOROTOR and <= MAVLink.MAV_TYPE.VTOL_RESERVED5
                        => DroneFirmware.Plane,
                    MAVLink.MAV_TYPE.QUADROTOR
                        or MAVLink.MAV_TYPE.TRICOPTER
                        or MAVLink.MAV_TYPE.HEXAROTOR
                        or MAVLink.MAV_TYPE.OCTOROTOR
                        or MAVLink.MAV_TYPE.HELICOPTER
                        or MAVLink.MAV_TYPE.DODECAROTOR
                        => DroneFirmware.Copter,
                    _ => DroneFirmware.Copter
                },
                MAVLink.MAV_AUTOPILOT.UDB when apType == MAVLink.MAV_TYPE.FIXED_WING => DroneFirmware.Plane,
                _ => DroneFirmware.Copter
            };
}