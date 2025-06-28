using NetTopologySuite.Geometries;
using System.Globalization;

namespace MistyMarkVisualize;

/// <summary>
/// Conversion utility for loading and saving coordinates.
/// </summary>
public static class CoordinateLoader
{
    // 0,0 is top left, so invert Y for human-readable coordinates (we don't need to digest a negative sign)

    /// <summary>
    /// Parses a collection of string representations of coordinates and populates a list with valid coordinates.
    /// </summary>
    /// <remarks>
    /// The method expects each string in <paramref name="lines"/> to be in the format "X,Y",
    /// where X and Y are floating-point numbers. If Y is negative, its absolute value is used to ensure human-readable coordinates.
    /// Invalid strings or strings with fewer than two parts are ignored.
    /// </remarks>
    /// <param name="lines">A collection of strings, each representing a coordinate in the format "X,Y".</param>
    /// <param name="list">A list to populate with valid <see cref="Coordinate"/> objects. Coordinates with invalid formats are ignored.</param>
    /// <returns><see langword="true"/> if at least one coordinate was successfully parsed and added to the list; otherwise, <see langword="false"/>.</returns>
    public static bool Load(IEnumerable<string> lines, List<Coordinate> list)
    {
        int count = 0;
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length >= 2 &&
                double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var x) &&
                double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var y))
            {
                list.Add(new Coordinate(x, Math.Abs(y)));
            }
            count++;
        }
        return count != 0;
    }

    /// <summary>
    /// Saves a list of coordinates by formatting and ordering them.
    /// </summary>
    /// <remarks>
    /// This method processes the provided list of coordinates by converting each coordinate into a formatted string in the form "X,-Y",
    /// where X and Y are the respective values of the coordinate.
    /// Negative Y values are explicitly formatted with a leading minus sign. Duplicate coordinates are removed, and the resulting collection is ordered.
    /// </remarks>
    /// <param name="list">The list of <see cref="Coordinate"/> objects to process.</param>
    /// <returns>An ordered collection of unique formatted coordinate strings. Each string represents a coordinate in the format "X,-Y".</returns>
    public static IOrderedEnumerable<string> Save(List<Coordinate> list)
    {
        // Get unique coordinates and format them
        var result = new HashSet<string>();
        foreach (var point in list)
            result.Add($"{point.X:R},{-point.Y:R}");
        return result.Order();
    }
}