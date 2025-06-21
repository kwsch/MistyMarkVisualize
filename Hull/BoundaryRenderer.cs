using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace MistyMarkVisualize;

public static class BoundaryRenderer
{
    public static Brush Regular { get; set; } = Brushes.Red;
    public static Brush Alternate { get; set; } = Brushes.Blue;

    public static Bitmap Render(string wkt,
        ReadOnlySpan<Coordinate> mistyPoints,
        ReadOnlySpan<Coordinate> nonMistyPoints,
        int width, int height,
        int padding = 0)
    {
        var reader = new WKTReader();
        var geom = reader.Read(wkt);

        return Render(geom, mistyPoints, nonMistyPoints, width, height, padding);
    }

    public static Bitmap Render(Geometry geom,
        ReadOnlySpan<Coordinate> mistyPoints,
        ReadOnlySpan<Coordinate> nonMistyPoints,
        int width, int height,
        int padding = 0)
    {
        const double scaleX = 1;
        const double scaleY = 1;
        var scale = Math.Min(scaleX, scaleY);

        // Center the drawing if aspect ratio doesn't match
        const double extraX = 0;
        const double extraY = 0;

        const double offsetX = 0;
        const double offsetY = 0;

        var bmp = new Bitmap(width, height);
        using var g = Graphics.FromImage(bmp);
        g.Clear(Color.Transparent);

        var blackPen = new Pen(Color.Black, 2);

        // Draw the alpha shape boundary
        if (geom is Polygon or MultiPolygon)
        {
            for (var i = 0; i < geom.NumGeometries; i++)
            {
                var poly = geom.GetGeometryN(i) as Polygon;
                if (poly != null)
                    DrawPolygon(poly, g, blackPen, offsetX, offsetY, scale, height, padding, extraX, extraY);
            }
        }

        // Draw Misty and non-Misty points
        DrawPoints(g, mistyPoints, Regular, offsetX, offsetY, scale, height, padding, extraX, extraY);
        DrawPoints(g, nonMistyPoints, Alternate, offsetX, offsetY, scale, height, padding, extraX, extraY);
        return bmp;
    }

    private static void DrawPolygon(Polygon polygon, Graphics g, Pen pen, double offsetX, double offsetY, double scale, int height, int padding, double extraX, double extraY)
    {
        var shell = polygon.Shell;
        var points = TransformCoordinates(shell.Coordinates, offsetX, offsetY, scale, height, padding, extraX, extraY);
        g.DrawPolygon(pen, points);

        var cyan = new Pen(Color.Cyan, 5);
        foreach (var hole in polygon.Holes)
        {
            var holePoints = TransformCoordinates(hole.Coordinates, offsetX, offsetY, scale, height, padding, extraX, extraY);
            g.DrawPolygon(cyan, holePoints);
        }
    }

    private static void DrawPoints(Graphics g, ReadOnlySpan<Coordinate> coords, Brush brush, double offsetX, double offsetY, double scale, int height, int padding, double extraX, double extraY)
    {
        foreach (var c in coords)
        {
            var x = ((c.X + offsetX) * scale) + padding + extraX;
            var y = height - (((c.Y + offsetY) * scale) + padding + extraY); // Flip Y
            g.FillEllipse(brush, (float)x - 2, (float)y - 2, 4, 4); // small dot
        }
    }

    private static PointF[] TransformCoordinates(Coordinate[] coords, double offsetX, double offsetY, double scale, int height, int padding, double extraX, double extraY)
    {
        var points = new PointF[coords.Length];
        for (var i = 0; i < coords.Length; i++)
        {
            var x = ((coords[i].X + offsetX) * scale) + padding + extraX;
            var y = height - (((coords[i].Y + offsetY) * scale) + padding + extraY); // Flip Y
            points[i] = new PointF((float)x, (float)y);
        }
        return points;
    }
}