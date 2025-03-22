namespace GpsTriangulation.Services;

/// <summary>
/// The GPS triangulator.
/// </summary>
/// <remarks>
/// This class implements the <see cref="IGpsTriangulator"/> interface and is used to perform GPS triangulation.
/// </remarks>
public class GpsTriangulator : IGpsTriangulator
{
    // Earth radius in feet (6371 km * 1000 * 3.28084)
    private const double EarthRadiusInFeet = 20902231.52;

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
    public Task<List<MatchedPair>> CalculateClosestStations(
        List<Dictionary<string, object>> baseData,
        List<Dictionary<string, object>> comparisonData,
        string baseLatColumn,
        string baseLonColumn,
        string targetLatColumn,
        string targetLonColumn,
        double maxDistance = 15)
    {
        var matchedPairs = new List<MatchedPair>();

        foreach (var baseRecord in baseData)
        {
            var baseLat = Convert.ToDouble(baseRecord[baseLatColumn].ToString());
            var baseLon = Convert.ToDouble(baseRecord[baseLonColumn].ToString());

            Dictionary<string, object>? closestRecord = null;
            double? closestDistance = null;
            double minDistance = double.MaxValue;

            foreach (var compRecord in comparisonData)
            {
                var compLat = Convert.ToDouble(compRecord[targetLatColumn].ToString());
                var compLon = Convert.ToDouble(compRecord[targetLonColumn].ToString());

                var distance = CalculateDistance(baseLat, baseLon, compLat, compLon);

                if (distance <= maxDistance && distance < minDistance)
                {
                    minDistance = distance;
                    closestRecord = compRecord;
                    closestDistance = distance;
                }
            }

            matchedPairs.Add(new MatchedPair
            {
                BaseRecord = baseRecord!,
                ClosestComparison = closestRecord!,
                DistanceInFeet = closestDistance
            });
        }

        return Task.FromResult(matchedPairs);
    }

    /// <summary>
    /// Calculates the distance between two geographical points specified by latitude and longitude using the Vincenty formula.
    /// </summary>
    /// <param name="lat1">The latitude of the first point in degrees.</param>
    /// <param name="lon1">The longitude of the first point in degrees.</param>
    /// <param name="lat2">The latitude of the second point in degrees.</param>
    /// <param name="lon2">The longitude of the second point in degrees.</param>
    /// <returns>A <see cref="Task{Double}"/> representing the asynchronous operation, with the distance between the two points in feet.</returns>
    public async Task<double> DistanceBetweenPoints(double lat1, double lon1, double lat2, double lon2)
    {
        var point1 = new GeoPoint() { Latitude = lat1, Longitude = lon1 };
        var point2 = new GeoPoint() { Latitude = lat2, Longitude = lon2 };
        var result = await GeodesicCalculator.CalculateVincenty(point1, point2);

        // Return the distance in feet.
        return result.Distance * 3.28084;
    }

    /// <summary>
    /// Calculates the distance between two geographical points specified by latitude and longitude using the Haversine formula.
    /// </summary>
    /// <param name="lat1">The latitude of the first point in degrees.</param>
    /// <param name="lon1">The longitude of the first point in degrees.</param>
    /// <param name="lat2">The latitude of the second point in degrees.</param>
    /// <param name="lon2">The longitude of the second point in degrees.</param>
    /// <returns>The distance between the two points in feet.</returns>
    private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        // Convert degrees to radians
        var lat1Rad = lat1 * Math.PI / 180.0;
        var lon1Rad = lon1 * Math.PI / 180.0;
        var lat2Rad = lat2 * Math.PI / 180.0;
        var lon2Rad = lon2 * Math.PI / 180.0;

        // Haversine formula
        var dLat = lat2Rad - lat1Rad;
        var dLon = lon2Rad - lon1Rad;
        var a = Math.Pow(Math.Sin(dLat / 2), 2) +
               Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
               Math.Pow(Math.Sin(dLon / 2), 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return EarthRadiusInFeet * c;
    }
}