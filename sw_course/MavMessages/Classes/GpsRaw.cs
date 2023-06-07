namespace sw_course.MavMessages.Classes;

public class GpsRaw : MavMessage
{
    public GpsRaw(MAVLink.MAVLinkMessage message) : base(message)
    {
        var gps = message.ToStructureGC<MAVLink.mavlink_gps_raw_int_t>();
                
        var mslAltitude = gps.alt / 1000.0f;
        var longitude = gps.lat * 1.0e-7f;
        var latitude = gps.lon * 1.0e-7f;
        var gpsStatus = gps.fix_type;
        var satellitesVisible = gps.satellites_visible;
        var groundcourse = gps.cog * 1.0e-2f;
        if (mslAltitude == MslAltitude &&
            longitude == Longitude &&
            latitude == Latitude &&
            gpsStatus == GpsStatus &&
            satellitesVisible == SatellitesVisible &&
            groundcourse == Groundcourse) 
            return;
        
        MslAltitude = mslAltitude;
        Longitude = longitude;
        Latitude = latitude;
        GpsStatus = gpsStatus;
        SatellitesVisible = satellitesVisible;
        Groundcourse = groundcourse;
        _isDifferent = true;
    }
}