namespace sw_course;

public static class Utils
{
    private static readonly int StxLenght = 1;
    public static BinaryReader GetStreamMessage(byte[] message)
    {
        var stream = new MemoryStream(MAVLink.MAVLINK_MAX_PACKET_LEN);
        stream.Write(message);
        stream.Position = 0;
        return new BinaryReader(stream);
    }

    public static byte[] GetHeartbeatMessage()
    {
        var structure = new MAVLink.mavlink_heartbeat_t(1, 2, 1, 0, 0, 0);
        var message = GenerateMav2Packet(MAVLink.MAVLINK_MSG_ID.HEARTBEAT, 1, 1, structure);
        return message;
    }
    
    public static byte[] GetEmptyMessage()
        => new byte[280];

    public static bool CheckCrc(MAVLink.MAVLinkMessage message, byte[] buffer)
    {
        var msgInfo = MAVLink.MAVLINK_MESSAGE_INFOS.GetMessageInfo(message.msgid);

        if (!message.ismavlink2 && message.payloadlength != msgInfo.minlength)
        {
            if (msgInfo.length == 0) // pass for unknown packets
                Console.WriteLine($"Unknown packet type: {message.msgid}");
            else
            {
                Console.WriteLine($"Mavlink bad packet (len fail) len: {message.payloadlength}, pkno: {message.msgid}");
                return false;
            }    
        }

        var sigSize = message.sig is {Length: > 0} ? MAVLink.MAVLINK_SIGNATURE_BLOCK_LEN : 0;
        ushort crc = MAVLink.MavlinkCRC.crc_calculate(buffer, message.Length - sigSize - MAVLink.MAVLINK_NUM_CHECKSUM_BYTES);
        crc = MAVLink.MavlinkCRC.crc_accumulate(msgInfo.crc, crc);

        return (message.crc16 >> 8) != (crc >> 8) ||
               (message.crc16 & 0xff) != (crc & 0xff);
    }

    private static byte[] GenerateMav2Packet(MAVLink.MAVLINK_MSG_ID msgId, int sysId, int compId, object rawData, bool signing = false)
    {
        byte[] data = MavlinkUtil.StructureToByteArray(rawData);
        int lenghtPacket = signing
            ? data.Length + MAVLink.MAVLINK_CORE_HEADER_LEN + MAVLink.MAVLINK_NUM_CHECKSUM_BYTES + StxLenght + MAVLink.MAVLINK_SIGNATURE_BLOCK_LEN
            : data.Length + MAVLink.MAVLINK_CORE_HEADER_LEN + MAVLink.MAVLINK_NUM_CHECKSUM_BYTES + StxLenght;
        byte[] packet = new byte[lenghtPacket];

        MavlinkUtil.trim_payload(ref data);
        packet[0] = MAVLink.MAVLINK_STX;
        packet[1] = (byte)data.Length;
        packet[2] = 0; // incompat
        if (signing) // current mav
            packet[2] |= MAVLink.MAVLINK_IFLAG_SIGNED;
        packet[3] = 0; // compat
        packet[4] = (byte)1;
        packet[5] = (byte)sysId;
        packet[6] = (byte)compId;
        packet[7] = (byte)((long)msgId & 0xff);
        packet[8] = (byte)(((long)msgId >> 8) & 0xff);
        packet[9] = (byte)(((long)msgId >> 16) & 0xff);
        int i = 10;
        foreach (byte b in data)
        {
            packet[i] = b;
            i++;
        }
        ushort checksum = MAVLink.MavlinkCRC.crc_calculate(packet, packet[1] + MAVLink.MAVLINK_NUM_HEADER_BYTES);
        checksum = MAVLink.MavlinkCRC.crc_accumulate(MAVLink.MAVLINK_MESSAGE_INFOS.GetMessageInfo((uint)msgId).crc,
            checksum);

        var ckA = (byte)(checksum & 0xFF); //< High byte
        var ckB = (byte)(checksum >> 8); //< Low byte

        packet[i] = ckA;
        i++;
        packet[i] = ckB;

        return packet;
    }

}