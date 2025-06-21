using NetTopologySuite.Geometries;
using System.Globalization;

namespace MistyMarkVisualize;

public static class CoordinateLoader
{
    public static List<Coordinate> Load(IEnumerable<string> lines)
    {
        var list = new List<Coordinate>();
        Load(lines, list);
        return list;
    }

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
                list.Add(new Coordinate(x, y));
            }
            count++;
        }
        return count != 0;
    }
}