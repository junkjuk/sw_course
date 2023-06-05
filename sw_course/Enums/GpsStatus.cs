namespace sw_course.Enums
{
    public enum GpsStatus
    {
        NoGPS = 0,
        NoFix = 1,
        
        Fix2D = 2,
        Fix3D = 3,
        FixDgps = 4,
        
        RtkFloat = 5,
        RtkFixed = 6,
        StaticFixed = 7,
        
        PPP = 8
    }
}