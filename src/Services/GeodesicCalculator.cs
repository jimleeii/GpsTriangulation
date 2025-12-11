namespace GpsTriangulation.Services;

/// <summary>
/// The geodesic calculator.
/// </summary>
public static class GeodesicCalculator
{
    // WGS-84 Earth parameters
    private const double a = 6378137.0;        // Semi-major axis (meters)

    private const double b = 6356752.314245;    // Semi-minor axis (meters)
    private const double f = 1 / 298.257223563; // Flattening

    /// <summary>
    /// Calculates the distance and initial/final bearings between two points using the Vincenty formula.
    /// </summary>
    /// <param name="point1">The first point.</param>
    /// <param name="point2">The second point.</param>
    /// <param name="maxIterations">The maximum number of iterations to run before giving up. Defaults to 200.</param>
    /// <returns>A <see cref="GeodesicResult"/> containing the distance and bearings.</returns>
    /// <remarks>
    /// Based on the algorithm described at https://en.wikipedia.org/wiki/Vincenty%27s_formulae
    /// This is a synchronous CPU-bound operation.
    /// </remarks>
    public static GeodesicResult CalculateVincentySync(GeoPoint point1, GeoPoint point2, int maxIterations = 200)
    {
        var φ1 = ToRadians(point1.Latitude);
        var λ1 = ToRadians(point1.Longitude);
        var φ2 = ToRadians(point2.Latitude);
        var λ2 = ToRadians(point2.Longitude);

        var L = λ2 - λ1;
        var tanU1 = (1 - f) * Math.Tan(φ1);
        var cosU1 = 1 / Math.Sqrt(1 + tanU1 * tanU1);
        var sinU1 = tanU1 * cosU1;
        var tanU2 = (1 - f) * Math.Tan(φ2);
        var cosU2 = 1 / Math.Sqrt(1 + tanU2 * tanU2);
        var sinU2 = tanU2 * cosU2;

        double λ = L, λʹ, sinσ, cosσ, σ, sinα, cosSqα, cos2σM;
        int iterations = 0;

        do
        {
            var sinλ = Math.Sin(λ);
            var cosλ = Math.Cos(λ);
            sinσ = Math.Sqrt((cosU2 * sinλ) * (cosU2 * sinλ) +
                    (cosU1 * sinU2 - sinU1 * cosU2 * cosλ) *
                    (cosU1 * sinU2 - sinU1 * cosU2 * cosλ));
            if (sinσ == 0) return new GeodesicResult { Distance = 0 };

            cosσ = sinU1 * sinU2 + cosU1 * cosU2 * cosλ;
            σ = Math.Atan2(sinσ, cosσ);
            sinα = cosU1 * cosU2 * sinλ / sinσ;
            cosSqα = 1 - sinα * sinα;
            cos2σM = cosσ - 2 * sinU1 * sinU2 / cosSqα;

            if (double.IsNaN(cos2σM)) cos2σM = 0;
            var C = f / 16 * cosSqα * (4 + f * (4 - 3 * cosSqα));
            λʹ = λ;
            λ = L + (1 - C) * f * sinα *
                (σ + C * sinσ * (cos2σM + C * cosσ * (-1 + 2 * cos2σM * cos2σM)));
        }
        while (Math.Abs(λ - λʹ) > 1e-12 && ++iterations < maxIterations);

        if (iterations >= maxIterations)
            throw new Exception("Vincenty formula failed to converge");

        var uSq = cosSqα * (a * a - b * b) / (b * b);
        var A = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));
        var B = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));
        var Δσ = B * sinσ * (cos2σM + B / 4 * (cosσ * (-1 + 2 * cos2σM * cos2σM) -
                B / 6 * cos2σM * (-3 + 4 * sinσ * sinσ) * (-3 + 4 * cos2σM * cos2σM)));

        var distance = b * A * (σ - Δσ);
        var α1 = Math.Atan2(cosU2 * Math.Sin(λ), cosU1 * sinU2 - sinU1 * cosU2 * Math.Cos(λ));
        var α2 = Math.Atan2(cosU1 * Math.Sin(λ), -sinU1 * cosU2 + cosU1 * sinU2 * Math.Cos(λ));

        return new GeodesicResult
        {
            Distance = distance,
            InitialBearing = ToDegrees(α1),
            FinalBearing = ToDegrees(α2)
        };
    }

    /// <summary>
    /// Calculates the distance and initial/final bearings between two points using the Vincenty formula.
    /// </summary>
    /// <param name="point1">The first point.</param>
    /// <param name="point2">The second point.</param>
    /// <param name="maxIterations">The maximum number of iterations to run before giving up. Defaults to 200.</param>
    /// <returns>A <see cref="Task{GeodesicResult}"/> containing the distance and bearings.</returns>
    /// <remarks>
    /// Async wrapper for the synchronous Vincenty calculation.
    /// The calculation is CPU-bound and completes synchronously.
    /// </remarks>
    public static Task<GeodesicResult> CalculateVincenty(GeoPoint point1, GeoPoint point2, int maxIterations = 200)
    {
        return Task.FromResult(CalculateVincentySync(point1, point2, maxIterations));
    }

    /// <summary>
    /// Converts to the radians.
    /// </summary>
    /// <param name="degrees">The degrees.</param>
    /// <returns>A double</returns>
    private static double ToRadians(double degrees) => degrees * Math.PI / 180.0;

    /// <summary>
    /// Converts to the degrees.
    /// </summary>
    /// <param name="radians">The radians.</param>
    /// <returns>A double</returns>
    private static double ToDegrees(double radians) => radians * 180.0 / Math.PI;
}