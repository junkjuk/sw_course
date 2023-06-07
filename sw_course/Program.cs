using sw_course;

if (args.Length < 2)
    throw new ArgumentException("No input and output files specified");

var inputFileName = args[0];
var outputFileName = args[1];

if (!File.Exists(inputFileName))
    throw new FileNotFoundException("Input file not found");

var start = DateTime.Now;

using var reader = new ReaderFromFile(Path.Combine(Directory.GetCurrentDirectory(), inputFileName));

using var fs = new StreamWriter(outputFileName);
foreach (var messageLine in reader.GetMessagesInCsvFormat())
    fs.WriteLine(messageLine);


var end = DateTime.Now - start;

Console.WriteLine("Success!");
Console.WriteLine($"Time elapsed: {end.TotalSeconds} seconds");