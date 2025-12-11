namespace GpsTriangulation.Services;

/// <summary>
/// Interface for GPS triangulation operations.
/// </summary>
public interface IGpsTriangulator
{
    /// <summary>
    /// Calculates the closest stations from the comparison data for each base data record.
    /// </summary>
    /// <param name="baseData">The base data.</param>
    /// <param name="comparisonData">The comparison data.</param>
    /// <param name="baseLatColumn">The base latitude column.</param>
    /// <param name="baseLonColumn">The base longitude column.</param>
    /// <param name="targetLatColumn">The target latitude column.</param>
    /// <param name="targetLonColumn">The target longitude column.</param>
    /// <param name="maxDistance">The maximum distance in feet. Defaults to 15.</param>
    /// <returns>A list of <see cref="MatchedPair"/> objects.</returns>
    Task<List<MatchedPair>> CalculateClosestStations(
        List<Dictionary<string, object>> baseData,
        List<Dictionary<string, object>> comparisonData,
        string baseLatColumn,
        string baseLonColumn,
        string targetLatColumn,
        string targetLonColumn,
        double maxDistance = 15);

    /// <summary>
    /// Calculates the distance between two geographical points specified by latitude and longitude using the Vincenty formula.
    /// </summary>
    /// <param name="lat1">The latitude of the first point in degrees.</param>
    /// <param name="lon1">The longitude of the first point in degrees.</param>
    /// <param name="lat2">The latitude of the second point in degrees.</param>
    /// <param name="lon2">The longitude of the second point in degrees.</param>
    /// <returns>A <see cref="Task{Double}"/> representing the asynchronous operation, with the distance between the two points in feet.</returns>
    Task<double> DistanceBetweenPoints(double lat1, double lon1, double lat2, double lon2);
}
