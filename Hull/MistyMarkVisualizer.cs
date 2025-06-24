using NetTopologySuite.Geometries;

namespace MistyMarkVisualize;

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

    public static List<string> GetFilteredMistyList(ReadOnlySpan<Coordinate> misty, ReadOnlySpan<Coordinate> alternate, double tolerance)
    {
        var hull = GetHull(misty, tolerance);

        var filteredPoints = new HashSet<string>();
        var factory = new GeometryFactory();
        foreach (var point in alternate)
        {
            var geomPoint = factory.CreatePoint(point);
            if (!hull.Contains(geomPoint))
                continue;
            filteredPoints.Add($"{point.X:R},{point.Y:R}");
        }
        return filteredPoints.OrderBy(z => z).ToList();
    }

    private static Geometry GetHull(ReadOnlySpan<Coordinate> points, double tolerance)
    {
        var envelope = new Envelope(new Coordinate(SizeX, SizeY)); // map size
        return MistyMarkHullBuilder.GetConcaveHull(points, envelope, tolerance);
    }
}

public record MapVisualization
{
    public required Bitmap Image { get; init; }
    public required Geometry Hull { get; init; }
}