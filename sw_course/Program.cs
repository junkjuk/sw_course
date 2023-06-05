// See https://aka.ms/new-console-template for more information

using sw_course;

var outFileName = "test.csv";

using var reader = new ReaderFromFile(Path.Combine(Directory.GetCurrentDirectory(), "testLog.tlog"));

using var fs = new StreamWriter(outFileName);
foreach (var messageLine in reader.GetMessagesInCsvFormat())
{
    fs.WriteLine(messageLine);
}