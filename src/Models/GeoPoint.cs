namespace GpsTriangulation.Models;

/// <summary>
/// Represents a geographical point with latitude and longitude.
/// </summary>
/// <param name="latitude">The latitude of the geographical point.</param>
/// <param name="longitude">The longitude of the geographical point.</param>
public record GeoPoint
{
    /// <summary>
    /// Gets or sets the latitude of the geographical point.
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Gets or sets the longitude of the geographical point.
    /// </summary>
    public double Longitude { get; set; }
}
