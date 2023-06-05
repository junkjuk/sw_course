using sw_course.Enums;

namespace sw_course;

public class MavCommon
{
    public static List<KeyValuePair<int, string>> GetModesList(DroneFirmware droneFirmware)
    {
        if (droneFirmware == DroneFirmware.Plane)
        {
            var names = typeof(PlaneFlightMode).GetEnumNames();
            var values = typeof(PlaneFlightMode).GetEnumValues().Cast<int>().ToArray();
            var temp = values.Select((t, i) => new KeyValuePair<int, string>(t, names[i])).ToList();

            return temp;
        }
        else if (droneFirmware == DroneFirmware.Copter)
        {
            var names =typeof(CopterFlightMode).GetEnumNames();
            var values = typeof(CopterFlightMode).GetEnumValues().Cast<int>().ToArray();
            var temp = values.Select((t, i) => new KeyValuePair<int, string>(t, names[i])).ToList();

            return temp;
        } else if (droneFirmware == DroneFirmware.Sub)
        {
            var names =typeof(SubFlightMode).GetEnumNames();
            var values = typeof(SubFlightMode).GetEnumValues().Cast<int>().ToArray();
            var temp = values.Select((t, i) => new KeyValuePair<int, string>(t, names[i])).ToList();

            return temp;
        }
        return null;
    }
}