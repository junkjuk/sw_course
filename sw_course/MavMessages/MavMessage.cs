using System.Globalization;
using sw_course.Enums;

namespace sw_course.MavMessages;

public abstract class MavMessage
{
    protected static uint msgid;
    private static DateTime Time;
    protected static string Mode = "";

    protected static DroneFirmware DroneFirmware;
    
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

        
    protected static MAVLink.MAV_AUTOPILOT _apName;
    protected static MAVLink.MAV_TYPE _apType;
    
    protected bool _isDifferent;
    protected bool _isIncorrectData;
    
    public MavMessage(MAVLink.MAVLinkMessage message)
    {
        msgid = message.msgid;
        var time = message.rxtime;
        if (Time - time > new TimeSpan(1,0,0,0,0))
        {
            _isIncorrectData = true;
            time = Time;
        }
        Time = time;
    }

    public DateTime GetTime => Time;
    

    public string GetCsv()
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