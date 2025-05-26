namespace MistyMarkVisualize;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        NUD_Tolerance.ValueChanged += ChangeTolerance;
        UpdateImage();
    }

    private void ChangeTolerance(object? sender, EventArgs e) => UpdateImage();

    private void UpdateImage()
    {
        var tolerance = (double)NUD_Tolerance.Value;
        var bmp = MistyMarkVisualizer.GetImage(Program.Coordinates, tolerance);
        PB_Image.Image = bmp;
    }
}