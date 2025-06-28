using NetTopologySuite.Geometries;

namespace MistyMarkVisualize;

/// <summary>
/// Visualizes the map of valid misty mark coordinates by generating a hull containing all points, and rendering it as an image.
/// </summary>
public static class MistyMarkVisualizer
{
    public const int SizeX = 2000;
    public const int SizeY = 2000;

    public static MapVisualization GetImage(ReadOnlySpan<Coordinate> misty, double tolerance, ReadOnlySpan<Coordinate> alternate = default)
    {
        var hull = GetHull(misty, tolerance);
        var img = BoundaryRenderer.Render(hull, misty, alternate, SizeX, SizeY);
        return new MapVisualization
        {
            Image = img,
            Hull = hull,
        };
    }

    public static IOrderedEnumerable<string> GetFilteredMistyList(ReadOnlySpan<Coordinate> misty, ReadOnlySpan<Coordinate> alternate, double tolerance)
    {
        var hull = GetHull(misty, tolerance);
        var factory = new GeometryFactory();

        var list = new List<Coordinate>();
        foreach (var point in alternate)
        {
            var geomPoint = factory.CreatePoint(point);
            if (!hull.Contains(geomPoint))
                continue;
            list.Add(point);
        }
        return CoordinateLoader.Save(list);
    }

    private static Geometry GetHull(ReadOnlySpan<Coordinate> points, double tolerance)
    {
        var envelope = new Envelope(new Coordinate(SizeX, SizeY)); // map size
        return MistyMarkHullBuilder.GetConcaveHull(points, envelope, tolerance);
    }
}