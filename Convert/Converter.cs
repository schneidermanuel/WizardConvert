using System.Diagnostics;

namespace WizardConvert.Convert;

internal static class Converter
{
    public static void PerformConvertion(string command, string path, bool keepFiles)
    {
        Console.WriteLine("Dateiformat zum konvertieren eingeben ('png', 'webm')");
        var format = Console.ReadLine();
        Console.WriteLine("Zielformat eingeben ('jpg', 'mp4')");
        var target = Console.ReadLine();
        if (string.IsNullOrEmpty(format) || string.IsNullOrEmpty(target))
        {
            throw new ArgumentException("Invalid File Format");
        }
        var allFiles = GetFilesRecursice(path, format, target, command);
        Console.WriteLine($"Gefunden: {allFiles.Length} Dateien");

        foreach (var fileConvertCommand in allFiles)
        {
            Console.WriteLine(fileConvertCommand.Command);
        }

        Console.WriteLine("Um die Konvertierung zu starten, gebe 'Abfahrt' ein!");
        if (Console.ReadLine() != "Abfahrt")
        {
            throw new OperationCanceledException();
        }

        foreach (var file in allFiles)
        {
            var processStartInfo = new ProcessStartInfo();
            processStartInfo.UseShellExecute = true;
            processStartInfo.FileName = "CMD.exe";
            processStartInfo.Arguments = $"/C {file.Command}";
            Console.WriteLine(file.Command);
            Process.Start(processStartInfo);
        }
    }

    private static ConversionFile[] GetFilesRecursice(
        string parent,
        string format,
        string target,
        string command
    )
    {
        try
        {
            var fileInputs = Directory.GetFiles(parent, $"*.{format}");
            var files = fileInputs.Select(f => BuildFile(f, format, target, command));
            var filesRecursive = Directory
                .GetDirectories(parent)
                .SelectMany(dir => GetFilesRecursice(dir, format, target, command))
                .ToList();
            filesRecursive.AddRange(files);
            return filesRecursive.Where(f => !File.Exists(f.NewFileName)).ToArray();
        }
        catch (UnauthorizedAccessException)
        {
            return Array.Empty<ConversionFile>();
        }
    }

    private static ConversionFile BuildFile(
        string fileName,
        string format,
        string target,
        string commandTemplate
    )
    {
        var newFileName = fileName.Replace($".{format}", $".{target}");
        var command = commandTemplate.Replace("INFILE", fileName).Replace("OUTFILE", newFileName);
        return new ConversionFile(fileName, newFileName, command);
    }
}
