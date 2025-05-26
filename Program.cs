using NetTopologySuite.Geometries;

namespace MistyMarkVisualize;

internal static class Program
{
    public static List<Coordinate> Coordinates { get; private set; } = new();

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        string exeDir = AppDomain.CurrentDomain.BaseDirectory;
        string mistyPath = Path.Combine(exeDir, "misty.txt");
        if (!File.Exists(mistyPath))
        {
            MessageBox.Show($"Could not find file: {mistyPath}", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        var lines = File.ReadLines(mistyPath);
        Coordinates = CoordinateLoader.Load(lines);
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }
}