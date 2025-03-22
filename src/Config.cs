using System.Text.Json;

namespace GpsTriangulation;

/// <summary>
/// Configuration class for defining data contracts.
/// </summary>
public static class Config
{
    /// <summary>
    /// Returns the configuration data for the GPS triangulator.
    /// </summary>
    /// <returns>A JSON string representing the configuration data.</returns>
    /// <remarks>
    /// The configuration data is structured as a <see cref="GpsTriangulatorDataRequest"/> object.
    /// The object contains the base GPS data points, the target GPS data points, the column names for the latitude and longitude values in the base and target data, and the maximum distance in feet.
    /// </remarks>
    public static string GetConfig()
    {
        // Define Base Run
        var baseData = new List<Dictionary<string, object>>
        {
            new() { { "id", 1 }, { "base_latitude", 37.7749 }, { "base_longitude", -122.4194 } },
            new() { { "id", 2 }, { "base_latitude", 37.7750 }, { "base_longitude", -122.4193 } },
            new() { { "id", 3 }, { "base_latitude", 37.7747 }, { "base_longitude", -122.4191 } },
            new() { { "id", 4 }, { "base_latitude", 37.7755 }, { "base_longitude", -122.4188 } },
            new() { { "id", 5 }, { "base_latitude", 37.7760 }, { "base_longitude", -122.4175 } }
        };

        // Define Target Run
        var comparisonData = new List<Dictionary<string, object>>
        {
            new() { { "station_id", "STA-100" }, { "target_latitude", 37.77491 }, { "target_longitude", -122.41939 }, {"stationing", "Station Alpha"} },
            new() { { "station_id", "STA-200" }, { "target_latitude", 37.7750 }, { "target_longitude", -122.4195 }, {"stationing", "Station Bravo"} },
            new() { { "station_id", "STA-300" }, { "target_latitude", 37.7746 }, { "target_longitude", -122.4190 }, {"stationing", "Station Charlie"} },
            new() { { "station_id", "STA-400" }, { "target_latitude", 37.7755 }, { "target_longitude", -122.4188 }, {"stationing", "Station Delta"} },
            new() { { "station_id", "STA-500" }, { "target_latitude", 37.7800 }, { "target_longitude", -122.4100 }, {"stationing", "Station Echo"} }
        };

        // Define Attribute Columns
        var baseLatColumn = "base_latitude";
        var baseLonColumn = "base_longitude";
        var targetLatColumn = "target_latitude";
        var targetLonColumn = "target_longitude";
        var maxDistance = 15.0;

        // Structure the configuration
        var config = new GpsTriangulatorDataRequest
        {
            BaseData = baseData,
            ComparisonData = comparisonData,
            BaseLatColumn = baseLatColumn,
            BaseLonColumn = baseLonColumn,
            TargetLatColumn = targetLatColumn,
            TargetLonColumn = targetLonColumn,
            MaxDistance = maxDistance
        };

        return JsonSerializer.Serialize(config, options: new JsonSerializerOptions { WriteIndented = true });
    }
}
