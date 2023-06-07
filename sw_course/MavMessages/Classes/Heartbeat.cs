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
        switch (apName)
        {
            case MAVLink.MAV_AUTOPILOT.ARDUPILOTMEGA:
                switch (apType)
                {
                    case MAVLink.MAV_TYPE.FIXED_WING:
                        DroneFirmware = DroneFirmware.Plane;
                        break;
                    case MAVLink.MAV_TYPE n when (n >= MAVLink.MAV_TYPE.VTOL_TAILSITTER_DUOROTOR && n <= MAVLink.MAV_TYPE.VTOL_RESERVED5):
                        DroneFirmware = DroneFirmware.Plane;
                        break;
                    case MAVLink.MAV_TYPE.FLAPPING_WING:
                        DroneFirmware = DroneFirmware.Plane;
                        break;
                    case MAVLink.MAV_TYPE.QUADROTOR:
                        DroneFirmware = DroneFirmware.Copter;
                        break;
                    case MAVLink.MAV_TYPE.TRICOPTER:
                        DroneFirmware = DroneFirmware.Copter;
                        break;
                    case MAVLink.MAV_TYPE.HEXAROTOR:
                        DroneFirmware = DroneFirmware.Copter;
                        break;
                    case MAVLink.MAV_TYPE.OCTOROTOR:
                        DroneFirmware = DroneFirmware.Copter;
                        break;
                    case MAVLink.MAV_TYPE.HELICOPTER:
                        DroneFirmware = DroneFirmware.Copter;
                        break;
                    case MAVLink.MAV_TYPE.DODECAROTOR:
                        DroneFirmware = DroneFirmware.Copter;
                        break;
                }

                break;
            case MAVLink.MAV_AUTOPILOT.UDB:
                switch (apType)
                {
                    case MAVLink.MAV_TYPE.FIXED_WING:
                        DroneFirmware = DroneFirmware.Plane;
                        break;
                }

                break;
        }
        if (hb.type == (byte)MAVLink.MAV_TYPE.GCS)
        {
            // skip gcs hb's
            // only happens on log playback - and shouldnt get them here
        }
        else
        {
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
    }
}