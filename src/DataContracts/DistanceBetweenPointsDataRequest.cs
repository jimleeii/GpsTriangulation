namespace GpsTriangulation.DataContracts;

/// <summary>
/// Represents a request to calculate the distance between two geographical points.
/// </summary>
/// <remarks>
/// This data contract holds the required information for calculating the distance
/// between two points using their geographical coordinates.
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="DistanceBetweenPointsDataRequest"/> struct.
/// </remarks>
/// <param name="point1">The first geographical point.</param>
/// <param name="point2">The second geographical point.</param>
public struct DistanceBetweenPointsDataRequest(GeoPoint point1, GeoPoint point2)
{
    /// <summary>
    /// Gets or sets the first geographical point.
    /// </summary>
    public required GeoPoint Point1 { get; set; } = point1;

    /// <summary>
    /// Gets or sets the second geographical point.
    /// </summary>
    public required GeoPoint Point2 { get; set; } = point2;

    /// <summary>
    /// Validates that Point1 and Point2 are not null.
    /// </summary>
    /// <param name="errors">A reference to a list of validation error messages, if any.</param>
    /// <returns>Returns true if both points are valid; otherwise, false.</returns>
    public readonly bool Validate(ref List<string>? errors)
    {
        errors ??= [];
        if (Point1 == null) errors.Add("Point1 cannot be null");
        if (Point2 == null) errors.Add("Point2 cannot be null");
        if (errors.Count > 0)
        {
            return false;
        }

        return true;
    }
}
