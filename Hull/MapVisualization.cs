using NetTopologySuite.Geometries;

namespace MistyMarkVisualize;

/// <summary>
/// Results of the map visualization process, containing the rendered image and the hull geometry.
/// </summary>
public record MapVisualization
{
    public required Bitmap Image { get; init; }
    public required Geometry Hull { get; init; }
}