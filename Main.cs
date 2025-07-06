using NetTopologySuite.Geometries;
using System.Runtime.InteropServices;
using NetTopologySuite.IO;

namespace MistyMarkVisualize;

public partial class Main : Form
{
    // last rendered hull
    private readonly GeometryFactory _geometryFactory = new();
    private MapVisualization _map;

    // mouse hover coordinate tracking
    private bool _isCoordinateValid;
    private (int X, int Y) _currentCoordinate;

    private bool _kitakami = true;

    public Main()
    {
        InitializeComponent();
        FLP_Alternate.Parent = L_Coordinate.Parent = PB_Image;

        B_ExportCreated.Visible = B_ClearCreated.Visible = Program.Created.Count != 0;
        NUD_Tolerance.ValueChanged += ChangeTolerance;
        LoadOtherCoordinates();
        _map = UpdateImage();
    }

    private const string NoOtherCoordinates = "None";

    private void LoadOtherCoordinates()
    {
        var other = Path.Combine(Program.ExeDir, "other");
        if (Directory.Exists(other))
        {
            // Try to load coordinates from the "other" directory
            var files = Directory.GetFiles(other, "*.txt", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var lines = File.ReadLines(file);
                var coords = new List<Coordinate>();
                if (!CoordinateLoader.Load(lines, coords))
                    continue; // Skip empty or invalid files

                var fileName = Path.GetFileName(file);
                var dir = Path.GetDirectoryName(file);
                var dirFolderName = dir != null ? Path.GetFileName(dir) : null;
                Program.AllCoordinates.Add($"{dirFolderName}\\{fileName}", coords);
            }
        }

        Init(CB_Coordinates1);
        Init(CB_Coordinates2);
        void Init(ComboBox cb)
        {
            cb.Items.Add("None");
            cb.SelectedIndex = 0;

            string GetIndex(string name)
            {
                if (!name.Contains('\\'))
                    return string.Empty;
                var split = name.Split('\\');
                var dir = split[0];
                name = split[1];

                return name;
            }
            foreach (var set in Program.AllCoordinates.OrderBy(z => GetIndex(z.Key)).ThenBy(z => z.Key))
            {
                if (set.Value.Count == 0)
                    continue; // Skip empty coordinate sets
                cb.Items.Add(set.Key);
            }
            cb.SelectedIndexChanged += ToggleBaseCoords;
        }
    }

    private void ChangeTolerance(object? sender, EventArgs e) => UpdateImage();
    private void ToggleBaseCoords(object? sender, EventArgs e)
    {
        B_ExportIntersections.Visible = CB_Coordinates2.SelectedItem?.ToString() != NoOtherCoordinates;
        UpdateImage();
    }

    private MapVisualization UpdateImage()
    {
        var tolerance = (double)NUD_Tolerance.Value;

        var coord1 = Program.AllCoordinates.TryGetValue(CB_Coordinates1.SelectedItem.ToString(), out var list1);
        var set1 = coord1 ? CollectionsMarshal.AsSpan(list1) : ReadOnlySpan<Coordinate>.Empty;

        var coord2 = Program.AllCoordinates.TryGetValue(CB_Coordinates2.SelectedItem.ToString(), out var list2);
        var set2 = coord2 ? CollectionsMarshal.AsSpan(list2) : ReadOnlySpan<Coordinate>.Empty;

        var result = MistyMarkVisualizer.GetImage(set1, tolerance, set2);
        PB_Image.Image?.Dispose();
        PB_Image.Image = result.Image;
        return _map = result;
    }

    private void B_ExportIntersections_Click(object sender, EventArgs e)
    {
        // Get other coordinate set
        ReadOnlySpan<Coordinate> alternate = [.. Program.Regular, .. Program.Outbreak];
        if (alternate.Length == 0)
        {
            MessageBox.Show("No alternate coordinates selected for intersection export.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var tolerance = (double)NUD_Tolerance.Value;
        var mist = CollectionsMarshal.AsSpan(Program.MistyCoordinates);
        var list = MistyMarkVisualizer.GetFilteredMistyList(mist, alternate, tolerance);

        var filePath = Path.Combine(Program.ExeDir, "all_fog.txt");
        var hullPath = Path.Combine(Program.ExeDir, "hull.txt");
        File.WriteAllLines(filePath, list);
        File.WriteAllText(hullPath, new WKTWriter().Write(_map.Hull));
        System.Media.SystemSounds.Asterisk.Play();
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

    private void TrackCoordinate(object sender, MouseEventArgs e)
    {
        _isCoordinateValid = TryGetCoordinate(PB_Image, e, out _currentCoordinate);
        L_Coordinate.Text = _isCoordinateValid ? $"X: {_currentCoordinate.X}, Y: {_currentCoordinate.Y}" : "Out of bounds.";
    }

    private void PB_Image_Click(object sender, EventArgs e)
    {
        if (!_isCoordinateValid)
        {
            System.Media.SystemSounds.Asterisk.Play();
            return;
        }

        var coord = _currentCoordinate;
        var fake = new Coordinate(coord.X, coord.Y);
        var point = _geometryFactory.CreatePoint(fake);

        // shift: add outside hull
        // shift-ctrl: add inside hull
        // alt: delete

        // otherwise, play sound (asterisk outside hull, hand inside hull)

        if (!_map.Hull.Contains(point) || (ModifierKeys & Keys.Control) != 0)
        {
            // Add
            if ((ModifierKeys & Keys.Shift) != 0)
                AddCoordinate(fake);
            else
                System.Media.SystemSounds.Asterisk.Play();
        }
        else
        {
            if ((ModifierKeys & Keys.Alt) != 0)
                RemoveCoordinate(fake);
            else
                System.Media.SystemSounds.Hand.Play();
        }
    }

    private void RemoveCoordinate(Coordinate fake)
    {
        var nearest = Program.Created.MinBy(z => z.Distance(fake));
        var distance = double.MaxValue;
        if (nearest == null || (distance = nearest.Distance(fake)) > (double)NUD_Tolerance.Value)
        {
            // Nothing really near the clicked location; be nice and show the nearest coordinate as a hint.
            var nearPosition = $"Nearest: {fake.X},{fake.Y} @ distance: {distance:F1}";
            MessageBox.Show($"No nearby created coordinate to remove. {nearPosition}", "Remove Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Program.Created.Remove(nearest);
        Program.MistyCoordinates.Remove(nearest);
        UpdateImage();
        if (Program.Created.Count == 0)
            B_ClearCreated.Visible = false;
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void AddCoordinate(Coordinate fake)
    {
        Program.Created.Add(fake);
        Program.MistyCoordinates.Add(fake);
        UpdateImage();
        B_ClearCreated.Visible = true;
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_ExportCreated_Click(object sender, EventArgs e)
    {
        if (Program.Created.Count == 0)
        {
            MessageBox.Show("No created coordinates to export.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var filePath = Path.Combine(Program.ExeDir, Program.FileNameCreated);
        try
        {
            File.WriteAllLines(filePath, CoordinateLoader.Save(Program.Created));
            MessageBox.Show($"Created coordinates exported to: {filePath}", "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to export created coordinates: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void B_ClearCreated_Click(object sender, EventArgs e)
    {
        if (Program.Created.Count == 0)
        {
            MessageBox.Show("No created coordinates to clear.", "Clear Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        Program.MistyCoordinates.RemoveAll(Program.Created.Contains);
        Program.Created.Clear();
        UpdateImage();
        MessageBox.Show("Created coordinates cleared.", "Clear Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        B_ClearCreated.Visible = false;
    }

    private void B_BB_Click(object sender, EventArgs e)
    {
        _kitakami ^= true; // Toggle between Kitakami and Terarium
        PB_Image.BackgroundImage = _kitakami ? Properties.Resources.kitakami2000 : Properties.Resources.terarium;
    }

    private void B_OpenCoords_Click(object sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog();
        ofd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
        ofd.Title = "Open Coordinates File";

        if (ofd.ShowDialog() != DialogResult.OK)
            return;

        var filePath = ofd.FileName;
        try
        {
            var lines = File.ReadAllLines(filePath);
            var list = Program.Outbreak;
            list.Clear();

            var points = CoordinateLoader.Load(lines, list);
            if (!points)
            {
                MessageBox.Show($"No points loaded from {filePath}.", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            UpdateImage();
            MessageBox.Show($"Coordinates loaded from: {filePath}", "Load Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load coordinates: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}