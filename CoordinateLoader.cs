using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.Operation.Union;
using System.Globalization;

namespace MistyMarkVisualize;

public static class CoordinateLoader
{
    public static List<Coordinate> Load(IEnumerable<string> lines)
    {
        var list = new List<Coordinate>();
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length >= 2 &&
                double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double x) &&
                double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double y))
            {
                list.Add(new Coordinate(x, y));
            }
        }
        return list;
    }
}

public class MistyMarkHullBuilder
{
    private readonly GeometryFactory _geometryFactory = new();

    public Geometry GetConcaveHull(List<Coordinate> points, Envelope planeBounds, double alpha)
    {
        var mapEnvelope = new Envelope(0, planeBounds.MaxX, 0, planeBounds.MaxX);
        var mapPolygon = _geometryFactory.ToGeometry(mapEnvelope);
        var fogShapes = new List<Geometry>();
        double bufferRadius = alpha; // adjust this to control how far fog stretches inward

        foreach (var coord in points)
        {
            var point = _geometryFactory.CreatePoint(coord);
            var buffered = point.Buffer(bufferRadius);
            fogShapes.Add(buffered);
        }

        // 4. Union all fog shapes together
        var cascadedUnion = CascadedPolygonUnion.Union(fogShapes);

        // 5. Intersect with map bounds to limit fog within the map
        var finalFogArea = cascadedUnion.Intersection(mapPolygon);
        return finalFogArea;
    }

    public string GetWkt(Geometry geom) => new WKTWriter().Write(geom);
}

public static class MistyMarkVisualizer
{
    public static Bitmap GetImage(List<Coordinate> mistyPoints, double tolerance)
    {
        var builder = new MistyMarkHullBuilder();
        var envelope = new Envelope(new Coordinate(2000, 2000));
        var hull = builder.GetConcaveHull(mistyPoints, envelope, tolerance); // Tweak tolerance value
        return BoundaryRenderer.Render(hull, mistyPoints, [], envelope);
    }
}

public static class BoundaryRenderer
{
    public static Bitmap Render(string wkt,
        List<Coordinate> mistyPoints,
        List<Coordinate> nonMistyPoints,
        Envelope area,
        int padding = 0)
    {
        var reader = new WKTReader();
        var geom = reader.Read(wkt);

        return Render(geom, mistyPoints, nonMistyPoints, area, padding);
    }

    public static Bitmap Render(Geometry geom, 
        List<Coordinate> mistyPoints, 
        List<Coordinate> nonMistyPoints, 
        Envelope area,
        int padding = 0)
    {
        int width = (int)area.MaxX;
        int height = (int)area.MaxY;

        double scaleX = 1;
        double scaleY = 1;
        double scale = Math.Min(scaleX, scaleY);

        // Center the drawing if aspect ratio doesn't match
        double extraX = 0;
        double extraY = 0;

        double offsetX = 0;
        double offsetY = 0;

        var bmp = new Bitmap(width, height);
        using var g = Graphics.FromImage(bmp);
        g.Clear(Color.Transparent);

        var blackPen = new Pen(Color.Black, 2);

        // Draw the alpha shape boundary
        if (geom is Polygon or MultiPolygon)
        {
            for (int i = 0; i < geom.NumGeometries; i++)
            {
                var poly = geom.GetGeometryN(i) as Polygon;
                if (poly != null)
                    DrawPolygon(poly, g, blackPen, offsetX, offsetY, scale, height, padding, extraX, extraY);
            }
        }

        // Draw Misty and non-Misty points
        DrawPoints(g, mistyPoints, Brushes.Red, offsetX, offsetY, scale, height, padding, extraX, extraY);
        DrawPoints(g, nonMistyPoints, Brushes.Blue, offsetX, offsetY, scale, height, padding, extraX, extraY);
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

    private static void DrawPoints(Graphics g, List<Coordinate> coords, Brush brush, double offsetX, double offsetY, double scale, int height, int padding, double extraX, double extraY)
    {
        foreach (var c in coords)
        {
            double x = (c.X + offsetX) * scale + padding + extraX;
            double y = height - ((c.Y + offsetY) * scale + padding + extraY); // Flip Y
            g.FillEllipse(brush, (float)x - 2, (float)y - 2, 4, 4); // small dot
        }
    }

    private static PointF[] TransformCoordinates(Coordinate[] coords, double offsetX, double offsetY, double scale, int height, int padding, double extraX, double extraY)
    {
        var points = new PointF[coords.Length];
        for (int i = 0; i < coords.Length; i++)
        {
            double x = (coords[i].X + offsetX) * scale + padding + extraX;
            double y = height - ((coords[i].Y + offsetY) * scale + padding + extraY); // Flip Y
            points[i] = new PointF((float)x, (float)y);
        }
        return points;
    }
}