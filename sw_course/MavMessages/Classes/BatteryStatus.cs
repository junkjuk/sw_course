namespace sw_course.MavMessages.Classes;

public class BatteryStatus : MavMessage
{
    public BatteryStatus(MAVLink.MAVLinkMessage message) : base(message)
    {
        var bats = message.ToStructureGC<MAVLink.mavlink_battery_status_t>();
                
        var batteryVoltage = bats.voltages.Sum(a => a != ushort.MaxValue ? a / 1000.0 : 0);
        var batteryRemaining = bats.battery_remaining;
        var current = bats.current_battery / 100.0f;

        if (batteryRemaining == BatteryRemaining &&
            current == Current &&
            batteryVoltage == BatteryVoltage) return;
        
        BatteryRemaining = batteryRemaining;
        Current = current;
        BatteryVoltage = batteryVoltage;
        _isDifferent = true;
    }
}