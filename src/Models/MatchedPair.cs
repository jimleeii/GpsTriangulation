namespace GpsTriangulation.Models;

/// <summary>
/// Represents a matched pair of records with their associated distance.
/// </summary>
public class MatchedPair
{
    /// <summary>
    /// Gets or sets the base record.
    /// </summary>
    public required Dictionary<string, object> BaseRecord { get; set; }

    /// <summary>
    /// Gets or sets the closest comparison record.
    /// </summary>
    public required Dictionary<string, object> ClosestComparison { get; set; }

    /// <summary>
    /// Gets or sets the distance between the base and comparison records in feet.
    /// </summary>
    public double? DistanceInFeet { get; set; }
}
