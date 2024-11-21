namespace WhoAreYou.Helpers;

public static class File
{
    public static void WriteToFile(string path, List<string> content)
    {
        try
        {
            System.IO.File.WriteAllLines(path, content);
        }
        catch
        {
            throw new Exception("Failed to write to file.");
        }
    }

    public static void AppendToFile(string path, string content)
    {
        try
        {
            System.IO.File.AppendAllText(path, content + Environment.NewLine);
        }
        catch
        {
            throw new Exception("Failed to write to file.");
        }
    }

    public static string[] ReadFromFile(string path)
    {
        try
        {
            return System.IO.File.ReadAllLines(path);
        }
        catch
        {
            throw new Exception("Failed to read from file.");
        }
    }

    public static string GenerateFileName(string module, string extension = "txt")
    {
        return $"{DateTime.Now:yyyy;MMdd;HH;mm}_{module}.{extension}";
    }
}