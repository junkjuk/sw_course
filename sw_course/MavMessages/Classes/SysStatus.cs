namespace sw_course.MavMessages.Classes;

public class SysStatus : MavMessage
{
    public SysStatus(MAVLink.MAVLinkMessage message) : base(message)
    {
        var sysStatus = message.ToStructureGC<MAVLink.mavlink_sys_status_t>();
                
        var batteryVoltage = sysStatus.voltage_battery / 1000.0f;
        var batteryRemaining = sysStatus.battery_remaining;
        var current = sysStatus.current_battery / 100.0f;

        if (batteryRemaining == BatteryRemaining &&
            current == Current &&
            batteryVoltage == BatteryVoltage) 
            return;
        
        BatteryRemaining = batteryRemaining;
        Current = current;
        BatteryVoltage = batteryVoltage;
        _isDifferent = true;
    }
}