using System.Runtime.InteropServices;
using NetTopologySuite.Geometries;

namespace MistyMarkVisualize;

public partial class Form1 : Form
{
    private readonly bool _hasOtherCoordinates;
    private readonly GeometryFactory _geometryFactory = new();
    private Geometry _hull;

    public Form1()
    {
        InitializeComponent();
        FLP_Alternate.Parent = L_Coordinate.Parent = PB_Image;

        NUD_Tolerance.ValueChanged += ChangeTolerance;
        if (!LoadOtherCoordinates())
            FLP_Alternate.Visible = false;
        else
            _hasOtherCoordinates = true;
        _hull = UpdateImage();
    }

    private const string RegularCoordinates = "Regular Coordinates";
    private const string OutbreakCoordinates = "Outbreak Coordinates";

    private bool LoadOtherCoordinates()
    {
        if (Program.Regular.Count > 0)
            CB_OtherCoordinateSelect.Items.Add(RegularCoordinates);
        if (Program.Outbreak.Count > 0)
            CB_OtherCoordinateSelect.Items.Add(OutbreakCoordinates);
        CB_OtherCoordinateSelect.SelectedIndex = 0;
        CB_OtherCoordinateSelect.SelectedIndexChanged += ToggleBaseCoords;
        return CB_OtherCoordinateSelect.Items.Count != 0;
    }

    private void ChangeTolerance(object? sender, EventArgs e) => UpdateImage();
    private void ToggleBaseCoords(object? sender, EventArgs e) => UpdateImage();

    private Geometry UpdateImage()
    {
        var tolerance = (double)NUD_Tolerance.Value;

        var mist = CollectionsMarshal.AsSpan(Program.MistyCoordinates);

        ReadOnlySpan<Coordinate> alternate = [];
        if (_hasOtherCoordinates && CHK_RenderBaseCoordinates.Checked)
        {
            var source = CB_OtherCoordinateSelect.SelectedItem?.ToString();
            if (source == RegularCoordinates)
                alternate = CollectionsMarshal.AsSpan(Program.Regular);
            else if (source == OutbreakCoordinates)
                alternate = CollectionsMarshal.AsSpan(Program.Outbreak);
        }

        var result = MistyMarkVisualizer.GetImage(mist, tolerance, alternate);
        PB_Image.Image?.Dispose();
        PB_Image.Image = result.Image;
        return _hull = result.Hull;
    }

    private void B_ExportIntersections_Click(object sender, EventArgs e)
    {
        var tolerance = (double)NUD_Tolerance.Value;

        var mist = CollectionsMarshal.AsSpan(Program.MistyCoordinates);
        var alternate = CollectionsMarshal.AsSpan(Program.Outbreak);

        var list = MistyMarkVisualizer.GetFilteredMistyList(mist, alternate, tolerance);

        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "all_fog.txt");
        File.WriteAllLines(filePath, list);
    }

    private static bool TryGetCoordinate(PictureBox control, MouseEventArgs e, out (int X, int Y) result)
    {
        result = default;
        var img = control.Image;
        if (img == null)
            return false;

        double imageAspect = (double)img.Width / img.Height;
        double boxAspect = (double)control.Width / control.Height;

        int renderedWidth, renderedHeight, offsetX, offsetY;
        if (imageAspect > boxAspect)
        {
            renderedWidth = control.Width;
            renderedHeight = (int)(control.Width / imageAspect);
            offsetX = 0;
            offsetY = (control.Height - renderedHeight) / 2;
        }
        else
        {
            renderedHeight = control.Height;
            renderedWidth = (int)(control.Height * imageAspect);
            offsetX = (control.Width - renderedWidth) / 2;
            offsetY = 0;
        }

        if (e.X < offsetX || e.X >= offsetX + renderedWidth ||
            e.Y < offsetY || e.Y >= offsetY + renderedHeight)
        {
            return false;
        }

        double relX = (e.X - offsetX) / (double)renderedWidth;
        double relY = (e.Y - offsetY) / (double)renderedHeight;

        int x = (int)(relX * img.Width);
        int y = (int)(relY * img.Height);

        // Flip Y
        y = img.Height - y - 1;

        result = (X: x, Y: y);
        return true;
    }

    private bool IsCoordinateValid;
    private (int X, int Y) CurrentCoordinate = default;

    private void TrackCoordinate(object sender, MouseEventArgs e)
    {
        if (!TryGetCoordinate(PB_Image, e, out var value))
        {
            L_Coordinate.Text = "Out of bounds.";
            IsCoordinateValid = false;
        }
        else
        {
            L_Coordinate.Text = $"X: {value.X}, Y: {value.Y}";
            IsCoordinateValid = true;
            CurrentCoordinate = value;
        }
    }

    private void PB_Image_Click(object sender, EventArgs e)
    {
        if (!IsCoordinateValid)
        {
            System.Media.SystemSounds.Asterisk.Play();
            return;
        }

        var coord = CurrentCoordinate;
        var fake = _geometryFactory.CreatePoint(new Coordinate(coord.X, coord.Y));
        if (_hull.Contains(fake))
        {
            System.Media.SystemSounds.Asterisk.Play();
        }
        else
        {
            System.Media.SystemSounds.Hand.Play();
        }
    }
}