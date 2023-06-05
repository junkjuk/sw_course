// See https://aka.ms/new-console-template for more information

using sw_course;
var reader = new TLogReader();

Console.WriteLine("Hello, World!");
var a = Utils.GetHeartbeatMessage()
    .Concat(Utils.GetHeartbeatMessage().Concat(Utils.GetHeartbeatMessage())).ToArray();
var stream = Utils.GetStreamMessage(a);
var messages = reader.ReadFromLogFile(stream);

Console.WriteLine(messages.Count());