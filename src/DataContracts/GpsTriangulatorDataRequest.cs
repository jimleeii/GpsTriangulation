namespace GpsTriangulation.DataContracts;

/// <summary>
/// The GPS triangulator data request.
/// </summary>
/// <remarks>
/// This structure represents the request data for the GPS triangulator.
/// </remarks>
public struct GpsTriangulatorDataRequest
{
    /// <summary>
    /// The GPS triangulator data request.
    /// </summary>
    /// <remarks>
    /// The default column names are "lat" and "lon" for both base and target data.
    /// The default maximum distance is 15 feet.
    /// </remarks>
    public GpsTriangulatorDataRequest()
    {
        BaseLatColumn = "lat";
        BaseLonColumn = "lon";
        TargetLatColumn = "lat";
        TargetLonColumn = "lon";
        MaxDistance = 15;
    }

    /// <summary>
    /// The GPS triangulator base data.
    /// </summary>
    public required List<Dictionary<string, object>> BaseData { get; set; }

    /// <summary>
    /// The GPS triangulator comparison data.
    /// </summary>
    public required List<Dictionary<string, object>> ComparisonData { get; set; }

    /// <summary>
    /// The column name for the base latitude.
    /// </summary>
    /// <remarks>
    /// Defaults to "lat".
    /// </remarks>
    public string BaseLatColumn { get; set; }

    /// <summary>
    /// The column name for the base longitude.
    /// </summary>
    /// <remarks>
    /// Defaults to "lon".
    /// </remarks>
    public string BaseLonColumn { get; set; }

    /// <summary>
    /// The column name for the target latitude.
    /// </summary>
    /// <remarks>
    /// Defaults to "lat".
    /// </remarks>
    public string TargetLatColumn { get; set; }

    /// <summary>
    /// The column name for the target longitude.
    /// </summary>
    /// <remarks>
    /// Defaults to "lon".
    /// </remarks>
    public string TargetLonColumn { get; set; }

    /// <summary>
    /// The maximum distance in feet.
    /// </summary>
    /// <remarks>
    /// Defaults to 15 feet.
    /// </remarks>
    public double MaxDistance { get; set; }

    /// <summary>
    /// Validates the GPS triangulator data request.
    /// </summary>
    /// <param name="errors">A reference to a list of validation error messages, if any.</param>
    /// <returns>Returns true if the request data is valid; otherwise, false.</returns>
    /// <remarks>
    /// This method checks for non-null and non-empty values for BaseData and ComparisonData,
    /// as well as non-empty strings for BaseLatColumn, BaseLonColumn, TargetLatColumn, and TargetLonColumn.
    /// It also ensures MaxDistance is greater than zero. Any validation errors are added to the errors list.
    /// </remarks>
    public readonly bool Validate(ref List<string>? errors)
    {
        errors ??= [];

        if (BaseData == null || BaseData.Count == 0)
        {
            errors.Add("BaseData is required and cannot be empty.");
        }

        if (ComparisonData == null || ComparisonData.Count == 0)
        {
            errors.Add("ComparisonData is required and cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(BaseLatColumn))
        {
            errors.Add("BaseLatColumn is required and cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(BaseLonColumn))
        {
            errors.Add("BaseLonColumn is required and cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(TargetLatColumn))
        {
            errors.Add("TargetLatColumn is required and cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(TargetLonColumn))
        {
            errors.Add("TargetLonColumn is required and cannot be empty.");
        }

        if (MaxDistance <= 0)
        {
            errors.Add("MaxDistance must be greater than zero.");
        }
        if (errors.Count > 0)
        {
            return false;
        }

        return true;
    }
}
