namespace sw_course;

public class TLogReader
{
    private readonly object _locker = new();
    private DateTime _lastLogTime = DateTime.MinValue;


    public IEnumerable<MAVLink.MAVLinkMessage> ReadFromLogFile(BinaryReader playbackFile)
    {
        if ( playbackFile is not {BaseStream.CanRead: true})
            yield break;

        lock (_locker)
        {
            var tempBuffer = new byte[3];
            while (true)
            {
                if(playbackFile.BaseStream.Length == playbackFile.BaseStream.Position)
                    yield break;

                var dateArray = new byte[8];
                var localPacketTime = DateTime.MinValue;
                playbackFile.Read(dateArray, 0, dateArray.Length);
                Array.Reverse(dateArray);
                
                var packetTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var intTime = BitConverter.ToUInt64(dateArray, 0);
                if (dateArray[7] == 254 || dateArray[7] == 253)
                {
                    playbackFile.BaseStream.Seek(-8, SeekOrigin.Current);
                }
                else if(intTime / 1_000 / 1_000 / 60 / 60 < 9_999_999)
                {
                    packetTime = packetTime.AddMilliseconds(intTime / 1_000);
                    localPacketTime = packetTime.ToLocalTime();
                }

                
                do
                {
                    if(playbackFile.BaseStream.Length == playbackFile.BaseStream.Position)
                        yield break;
                    tempBuffer[0] = playbackFile.ReadByte();
                } while (tempBuffer[0] != 'U' && tempBuffer[0] != MAVLink.MAVLINK_STX_MAVLINK1 && tempBuffer[0] != MAVLink.MAVLINK_STX);

                tempBuffer[1] = playbackFile.ReadByte();
                int length = tempBuffer[1]
                             + (tempBuffer[0] == MAVLink.MAVLINK_STX ? MAVLink.MAVLINK_CORE_HEADER_LEN : MAVLink.MAVLINK_CORE_HEADER_MAVLINK1_LEN)
                             + MAVLink.MAVLINK_NUM_CHECKSUM_BYTES
                             + 1; // + 2 checksum

                tempBuffer[2] = playbackFile.ReadByte();
                if (tempBuffer[0] == MAVLink.MAVLINK_STX)
                {
                    if ((tempBuffer[2] & MAVLink.MAVLINK_IFLAG_SIGNED) > 0)
                        length += MAVLink.MAVLINK_SIGNATURE_BLOCK_LEN;
                }

                var buffer = new byte[length];
                Array.Copy(tempBuffer, buffer, 3);
                playbackFile.Read(buffer, 3, length - 3);
                
                _lastLogTime = localPacketTime;
                
                yield return new MAVLink.MAVLinkMessage(buffer, _lastLogTime);
            }
        }
    }
}