namespace sw_course;

public class ReaderFromFile : IDisposable
{
    private const string HeaderLine =
        "BoardId,Time,Mode,MSL altitude(m),AGL altitude(m),Latitude,Longitude,ZSpeed(m/s),XSpeed(m/s),YSpeed(m/s),Ground course(deg),Battery remaining(%),Battery voltage(mV),Battery current(cA),GPS mode,Satellites Visible,GPRS Signal level(dBm)\n";

    private readonly TLogReader _logReader = new();
    private readonly BinaryReader _reader;

    public ReaderFromFile(string pathToTLogFile)
    {
        if (!File.Exists(pathToTLogFile))
            throw new FileNotFoundException();
        _reader = new BinaryReader(File.Open(pathToTLogFile, FileMode.Open, FileAccess.Read));
    }

    public IEnumerable<string> GetMessagesInCsvFormat()
    {
        yield return HeaderLine;

        var messages = _logReader.ReadFromLogFile(_reader);

        var currentDate = new DateTime();
        var currentStr = "";
        foreach (var element in messages)
        {
            var ms = new CurrentMessage(element);

            if (CurrentMessage.Time.Second != currentDate.Second)
            {
                currentDate = CurrentMessage.Time;
                if (!string.IsNullOrEmpty(currentStr))
                    yield return currentStr;
            }

            currentStr = ms.BuildCsvString() != string.Empty ? ms.BuildCsvString() : currentStr;
        }
    }

    public void Dispose()
    {
        _reader.Dispose();
    }
}