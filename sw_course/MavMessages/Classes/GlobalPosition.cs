namespace sw_course.MavMessages.Classes;

public class GlobalPosition : MavMessage
{
    public GlobalPosition(MAVLink.MAVLinkMessage message) : base(message)
    {
        var loc = message.ToStructureGC<MAVLink.mavlink_global_position_int_t>();
        if (loc.lat == 0 || loc.lon == 0 || loc.lat == int.MaxValue || loc.lon == int.MaxValue) 
            return;
        
        var aglAltitude = loc.relative_alt / 1000.0f;
        var longitude = loc.lat / 10000000.0f;
        var latitude = loc.lon / 10000000.0f;
        var xSpeed = loc.vx;
        var ySpeed = loc.vy;
        var hSpeed = loc.vz;

        if (aglAltitude == AglAltitude &&
            longitude == Longitude &&
            latitude == Latitude &&
            xSpeed == XSpeed &&
            ySpeed == YSpeed &&
            hSpeed == HSpeed) 
            return;
        
        AglAltitude = aglAltitude;
        Longitude = longitude;
        Latitude = latitude;
        XSpeed = xSpeed;
        YSpeed = ySpeed;
        HSpeed = hSpeed;
        _isDifferent = true;
    }
}