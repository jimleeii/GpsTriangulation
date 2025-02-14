namespace GpsTriangulation.Models;

/// <summary>
/// Represents the result of a geodesic calculation.
/// </summary>
public class GeodesicResult
{
    /// <summary>
    /// The distance between the two points in meters.
    /// </summary>
    public double Distance { get; set; }
    /// <summary>
    /// The initial bearing from point 1 to point 2 in degrees.
    /// </summary>
    public double InitialBearing { get; set; }
    /// <summary>
    /// The final bearing from point 1 to point 2 in degrees.
    /// </summary>
    public double FinalBearing { get; set; }
}
