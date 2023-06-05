using System.Globalization;
using sw_course.Enums;

namespace sw_course;

public class CurrentMessage
{
    public static uint msgid;
    public static DateTime Time;
    public static string Mode = "";

    public static DroneFirmware DroneFirmware;
    
    public static uint _custom_mode;
    public static int BoardId;
    public static double MslAltitude;
    public static double AglAltitude;
    public static double Longitude;
    public static double Latitude;
    public static double HSpeed;
    public static double YSpeed;
    public static double XSpeed;
    public static int GpsStatus;
    public static int SatellitesVisible;
    public static double BatteryVoltage;
    public static double Current;
    public static int BatteryRemaining;
    public static int GprsSignalLevel;
    
    public static double Groundcourse;

        
    private static MAVLink.MAV_AUTOPILOT _apName;
    private static MAVLink.MAV_TYPE _apType;

    private bool _isDifferent;
    private bool _isIncorrectData;

    public CurrentMessage(MAVLink.MAVLinkMessage message)
    {
        msgid = message.msgid;
        var time = message.rxtime;
        if (Time - time > new TimeSpan(1,0,0,0,0))
        {
            _isIncorrectData = true;
            time = Time;
        }
        Time = time;
        switch (msgid)
        {
            case (uint)MAVLink.MAVLINK_MSG_ID.GPRS_MODULE_INFO:
            {
                var msgStruct = message.ToStructureGC<MAVLink.mavlink_gprs_module_info_t>();
                var boardId = msgStruct.id;
                if (BoardId != boardId)
                {
                    BoardId = msgStruct.id;
                    _isDifferent = true;
                }
            }
                break;
            case (uint) MAVLink.MAVLINK_MSG_ID.GPRS_MODULE_STATUS:
            {
                var msgStruct = message.ToStructureGC<MAVLink.mavlink_gprs_module_status_t>();
                var boardId = msgStruct.id;
                var gprsSignalLevel = msgStruct.signal_level;
                if (boardId != BoardId || GprsSignalLevel != gprsSignalLevel)
                {
                    BoardId = boardId;
                    GprsSignalLevel = gprsSignalLevel;
                    _isDifferent = true;
                }
            }
                break;
            case (uint) MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT:
            {
                var loc = message.ToStructureGC<MAVLink.mavlink_global_position_int_t>();
                if (loc.lat != 0 && loc.lon != 0 && loc.lat != int.MaxValue && loc.lon != int.MaxValue)
                {
                    var aglAltitude = loc.relative_alt / 1000.0f;
                    var longitude = loc.lat / 10000000.0f;
                    var latitude = loc.lon / 10000000.0f;
                    var xSpeed = loc.vx;
                    var ySpeed = loc.vy;
                    var hSpeed = loc.vz;

                    if (aglAltitude != AglAltitude ||
                        longitude != Longitude ||
                        latitude != Latitude ||
                        xSpeed != XSpeed ||
                        ySpeed != YSpeed ||
                        hSpeed != HSpeed )
                    {
                        AglAltitude = aglAltitude;
                        Longitude = longitude;
                        Latitude = latitude;
                        XSpeed = xSpeed;
                        YSpeed = ySpeed;
                        HSpeed = hSpeed;
                        _isDifferent = true;
                    }
                    
                }
            }
                break;
            case (uint) MAVLink.MAVLINK_MSG_ID.GPS_RAW_INT:
            {
                var gps = message.ToStructureGC<MAVLink.mavlink_gps_raw_int_t>();
                
                var mslAltitude = gps.alt / 1000.0f;
                var longitude = gps.lat * 1.0e-7f;
                var latitude = gps.lon * 1.0e-7f;
                var gpsStatus = gps.fix_type;
                var satellitesVisible = gps.satellites_visible;
                var groundcourse = gps.cog * 1.0e-2f;
                if (mslAltitude != MslAltitude ||
                    longitude != Longitude ||
                    latitude != Latitude ||
                    gpsStatus != GpsStatus ||
                    satellitesVisible != SatellitesVisible || 
                    groundcourse != Groundcourse)
                {
                    MslAltitude = mslAltitude;
                    Longitude = longitude;
                    Latitude = latitude;
                    GpsStatus = gpsStatus;
                    SatellitesVisible = satellitesVisible;
                    Groundcourse = groundcourse;
                    _isDifferent = true;
                }
            }
                break;

            case (uint)MAVLink.MAVLINK_MSG_ID.GPS2_RAW:

            {
                var gps = message.ToStructureGC<MAVLink.mavlink_gps2_raw_t>();

                var latitude = gps.lat * 1.0e-7;
                var longitude = gps.lon * 1.0e-7;
                var mslAltitude = gps.alt / 1000.0f;
                var gpsStatus = gps.fix_type;
                var satellitesVisible = gps.satellites_visible;
                if (mslAltitude != MslAltitude ||
                    longitude != Longitude ||
                    latitude != Latitude ||
                    gpsStatus != GpsStatus ||
                    satellitesVisible != SatellitesVisible)
                {
                    MslAltitude = mslAltitude;
                    Longitude = longitude;
                    Latitude = latitude;
                    GpsStatus = gpsStatus;
                    SatellitesVisible = satellitesVisible;
                    _isDifferent = true;
                }
            }

                break;
            case (uint) MAVLink.MAVLINK_MSG_ID.BATTERY_STATUS:
            {
                var bats = message.ToStructureGC<MAVLink.mavlink_battery_status_t>();
                
                var batteryVoltage = bats.voltages.Sum(a => a != ushort.MaxValue ? a / 1000.0 : 0);
                var batteryRemaining = bats.battery_remaining;
                var current = bats.current_battery / 100.0f;
                
                if (batteryRemaining != BatteryRemaining ||
                    current != Current ||
                    batteryVoltage != BatteryVoltage )
                {
                    BatteryRemaining = batteryRemaining;
                    Current = current;
                    BatteryVoltage = batteryVoltage;
                    _isDifferent = true;
                }
            }
                break;
            case (uint) MAVLink.MAVLINK_MSG_ID.SYS_STATUS:
            {
                var sysstatus = message.ToStructureGC<MAVLink.mavlink_sys_status_t>();
                
                var batteryVoltage = sysstatus.voltage_battery / 1000.0f;
                var batteryRemaining = sysstatus.battery_remaining;
                var current = sysstatus.current_battery / 100.0f;
                
                if (batteryRemaining != BatteryRemaining ||
                    current != Current ||
                    batteryVoltage != BatteryVoltage )
                {
                    BatteryRemaining = batteryRemaining;
                    Current = current;
                    BatteryVoltage = batteryVoltage;
                    _isDifferent = true;
                }
            }
                break;
            case (uint) MAVLink.MAVLINK_MSG_ID.HEARTBEAT:
            {
                var hb = message.ToStructureGC<MAVLink.mavlink_heartbeat_t>();
                var ApName = (MAVLink.MAV_AUTOPILOT) hb.autopilot;
                var ApType = (MAVLink.MAV_TYPE) hb.type;
                var custom_mode = hb.custom_mode;

                if (ApName == _apName &&
                    ApType == _apType &&
                    custom_mode == _custom_mode)
                {
                    _isDifferent = true;
                    return;
                }                
                 switch (ApName)
                {
                    case MAVLink.MAV_AUTOPILOT.ARDUPILOTMEGA:
                        switch (ApType)
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
                        switch (ApType)
                        {
                            case MAVLink.MAV_TYPE.FIXED_WING:
                                DroneFirmware = DroneFirmware.Plane;
                                break;
                        }

                        break;
                    default:
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
                    foreach (var pair in modesList)
                    {
                        if (pair.Key == hb.custom_mode)
                        {
                            Mode = pair.Value;
                            break;
                        }
                    }
                }
            }
                break;
        }
    }

    public string BuildCsvString()
    {
        if (!_isDifferent)
            return string.Empty;
        
        var retStr = "";
        retStr += $"{(BoardId != 0 ? BoardId.ToString(CultureInfo.InvariantCulture) : "")},";
        retStr += $"{Time:yyyy-MM-dd HH:mm:ss},";
        retStr += $"{(Mode == "" ? _custom_mode != 0 ? _custom_mode.ToString(CultureInfo.InvariantCulture) : "" : Mode.ToString(CultureInfo.InvariantCulture))},";
        retStr += $"{(MslAltitude != 0 ? MslAltitude.ToString(CultureInfo.InvariantCulture): "")},";
        retStr += $"{(AglAltitude != 0 ? AglAltitude.ToString(CultureInfo.InvariantCulture): "")},";
        retStr += $"{(Latitude != 0 ? Latitude.ToString(CultureInfo.InvariantCulture) : "")},";
        retStr += $"{(Longitude != 0 ? Longitude.ToString(CultureInfo.InvariantCulture) : "")},";
        retStr += $"{(HSpeed / 100).ToString(CultureInfo.InvariantCulture)},";
        retStr += $"{(XSpeed / 100).ToString(CultureInfo.InvariantCulture)},";
        retStr += $"{(YSpeed / 100).ToString(CultureInfo.InvariantCulture)},";
        retStr += $"{(Groundcourse != 0 ? Groundcourse.ToString(CultureInfo.InvariantCulture) : "")},";
        retStr += $"{(BatteryRemaining != 0 ? BatteryRemaining.ToString(CultureInfo.InvariantCulture) : "")},";
        retStr += $"{(BatteryVoltage != 0 ? BatteryVoltage.ToString(CultureInfo.InvariantCulture) : "")},";
        retStr += $"{(Current != 0 ? Current.ToString(CultureInfo.InvariantCulture) : "")},";
        retStr += $"{(GpsStatus != 0 ? ((GpsStatus) GpsStatus).ToString() : "")},";
        retStr += $"{(SatellitesVisible != 0 ? SatellitesVisible.ToString(CultureInfo.InvariantCulture) : "")},";
        retStr += $"{(GprsSignalLevel != 0 ? GprsSignalLevel.ToString(CultureInfo.InvariantCulture) : "")}";

        if(_isIncorrectData)
            retStr += $",Incorrect data";
        
        return retStr;
    }
}