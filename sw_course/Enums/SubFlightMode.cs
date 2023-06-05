namespace sw_course.Enums;

public enum SubFlightMode
{
    /// <summary>manual angle with manual depth/throttle</summary>
    Stabilize = 0,
    /// <summary> manual body-frame angular rate with manual depth/throttle</summary>
    Acro = 1,
    /// <summary>manual angle with automatic depth/throttle</summary>
    AltHold = 2,
    /// <summary>not implemented in sub // fully automatic waypoint control using mission commands</summary>
    Auto = 3,
    /// <summary>fully automatic fly to coordinate or fly at velocity/direction using GCS immediate commands</summary>
    Guided = 4,
    /// <summary>automatic circular flight with automatic throttle</summary>
    Circle = 7,
    /// <summary>automatically return to surface, pilot maintains horizontal control</summary>
    Surface = 9,
    /// <summary>automatic position hold with manual override, with automatic throttle</summary>
    Poshold = 16,
    /// <summary>Pass-through input with no stabilization</summary>
    Manual = 19
}