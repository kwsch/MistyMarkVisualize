using NetTopologySuite.Geometries;

namespace MistyMarkVisualize;

internal static class Program
{
    public static readonly List<Coordinate> MistyCoordinates = [];
    public static readonly List<Coordinate> Regular = [];
    public static readonly List<Coordinate> Outbreak = [];

    private const string FileNameMisty = "misty.txt";
    private const string FileNameRegular = "original_encount.txt";
    private const string FileNameOutbreak = "original_outbreak.txt";

    public static readonly string ExeDir = Path.GetDirectoryName(Environment.ProcessPath)!;

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        // Load files from the same directory as the executable
        var exeDir = ExeDir;

        // Load the Misty coordinates from file
        if (!TryLoadMisty(exeDir))
            return;

        // If alternate coordinates are available, load them
        TryLoad(exeDir, FileNameRegular, Regular);
        TryLoad(exeDir, FileNameOutbreak, Outbreak);

        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }

    private static bool TryLoadMisty(string folderName)
    {
        var mistyPath = Path.Combine(folderName, FileNameMisty);
        if (!File.Exists(mistyPath))
        {
            MessageBox.Show($"Could not find file: {mistyPath}", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        var lines = File.ReadLines(mistyPath);
        if (!CoordinateLoader.Load(lines, MistyCoordinates))
        {
            MessageBox.Show($"Failed to load coordinates from: {mistyPath}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        return true;
    }

    private static bool TryLoad(string folderName, string fileName, List<Coordinate> regular)
    {
        var regularPath = Path.Combine(folderName, fileName);
        if (!File.Exists(regularPath))
            return false;
        var lines = File.ReadLines(regularPath);
        CoordinateLoader.Load(lines, regular);
        return true;
    }
}