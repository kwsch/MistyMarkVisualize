using NetTopologySuite.Geometries;
using NetTopologySuite.Operation.Union;

namespace MistyMarkVisualize;

public static class MistyMarkHullBuilder
{
    private static readonly GeometryFactory Factory = new();

    /// <summary>
    /// Generates a concave hull geometry by buffering input points and combining them into a single shape.
    /// </summary>
    /// <remarks>This method creates a concave hull by buffering each input point with the specified radius,
    /// combining the buffered shapes using a union operation, and intersecting the result with the plane bounds. The
    /// resulting geometry is limited to the area defined by <paramref name="planeBounds"/>.</remarks>
    /// <param name="points">A read-only span of coordinates representing the points to be included in the hull.</param>
    /// <param name="planeBounds">The bounding envelope of the plane, used to constrain the resulting geometry.</param>
    /// <param name="bufferRadius">The radius used to buffer each point, influencing the shape and size of the resulting hull.</param>
    /// <returns>A <see cref="Geometry"/> object representing the concave hull, constrained within the specified plane bounds.</returns>
    public static Geometry GetConcaveHull(ReadOnlySpan<Coordinate> points, Envelope planeBounds, double bufferRadius)
    {
        var mapEnvelope = new Envelope(0, planeBounds.MaxX, 0, planeBounds.MaxX);
        var mapPolygon = Factory.ToGeometry(mapEnvelope);
        if (points.Length == 0)
            return mapPolygon;
        var fogShapes = new List<Geometry>();

        foreach (var coordinate in points)
        {
            var point = Factory.CreatePoint(coordinate);
            var buffered = point.Buffer(bufferRadius);
            fogShapes.Add(buffered);
        }

        // 4. Union all fog shapes together
        var cascadedUnion = CascadedPolygonUnion.Union(fogShapes);

        // 5. Intersect with map bounds to limit fog within the map
        return cascadedUnion.Intersection(mapPolygon);
    }
}