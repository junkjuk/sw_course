namespace sw_course.MavMessages.Classes;

public class Gps2Raw : MavMessage
{
    public Gps2Raw(MAVLink.MAVLinkMessage message) : base(message)
    {
        var gps = message.ToStructureGC<MAVLink.mavlink_gps2_raw_t>();

        var latitude = gps.lat * 1.0e-7;
        var longitude = gps.lon * 1.0e-7;
        var mslAltitude = gps.alt / 1000.0f;
        var gpsStatus = gps.fix_type;
        var satellitesVisible = gps.satellites_visible;
        if (mslAltitude == MslAltitude &&
            longitude == Longitude &&
            latitude == Latitude &&
            gpsStatus == GpsStatus &&
            satellitesVisible == SatellitesVisible) 
            return;
        
        MslAltitude = mslAltitude;
        Longitude = longitude;
        Latitude = latitude;
        GpsStatus = gpsStatus;
        SatellitesVisible = satellitesVisible;
        _isDifferent = true;
    }
}