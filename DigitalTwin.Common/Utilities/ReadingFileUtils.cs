namespace DigitalTwin.Common.Utilities;

public static class ReadingFileUtils
{
    public static string ReadingScriptFile(string fileName)
    {
        var buildDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        var filePath = buildDir + @$"\SQLScript\{fileName}";

        var file = File.ReadAllText(filePath);
        return file;
    }
}