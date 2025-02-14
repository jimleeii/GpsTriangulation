namespace GpsTriangulation.EndpointDefinitions;

/// <summary>
/// The GPS triangulator endpoint definition.
/// </summary>
/// <remarks>
/// This class implements the <see cref="IEndpointDefinition"/> interface and is used to define the
/// GPS triangulator endpoint. The endpoint is registered as a POST endpoint at /api/GpsTriangulate.
/// </remarks>
public class GpsTriangulatorEndpointDefinition : IEndpointDefinition
{
    /// <summary>
    /// Defines the endpoints.
    /// </summary>
    /// <remarks>
    /// The GPS triangulator endpoint is registered as a POST endpoint at /api/GpsTriangulate.
    /// </remarks>
    /// <param name="app">The app.</param>
    /// <param name="evnt">The environment.</param>
    public void DefineEndpoints(WebApplication app, IWebHostEnvironment evnt)
    {
        app.MapPost("api/GpsTriangulate", GpsTriangulateAsync);
        app.MapPost("api/DistanceBetweenPoints", DistanceBetweenPointsAsync);
    }

    /// <summary>
    /// Defines the services required for GPS triangulation functionality.
    /// </summary>
    /// <param name="services">The service collection to which the services are added.</param>
    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IGpsTriangulator, GpsTriangulator>();
    }

    /// <summary>
    /// Handles the GPS triangulation request and returns the result.
    /// </summary>
    /// <param name="gpsTriangulator">The GPS triangulator.</param>
    /// <param name="request">The GPS triangulator data request.</param>
    /// <returns>The result of the GPS triangulation.</returns>
    private async Task<IResult> GpsTriangulateAsync(IGpsTriangulator gpsTriangulator, GpsTriangulatorDataRequest request)
    {
        var errors = new List<string>();
		if (!request.Validate(ref errors))
		{
			Results.BadRequest(string.Join("\n", errors!));
		}

        var matchedPairs = await gpsTriangulator.CalculateClosestStations(
            request.BaseData,
            request.ComparisonData,
            request.BaseLatColumn,
            request.BaseLonColumn,
            request.TargetLatColumn,
            request.TargetLonColumn,
            request.MaxDistance);
        return Results.Ok(matchedPairs);
    }

    /// <summary>
    /// Handles the distance between two points request and returns the result.
    /// </summary>
    /// <param name="gpsTriangulator">The GPS triangulator.</param>
    /// <param name="request">The distance between two points data request.</param>
    /// <returns>The result of the distance between the two points.</returns>
    private async Task<IResult> DistanceBetweenPointsAsync(IGpsTriangulator gpsTriangulator, DistanceBetweenPointsDataRequest request)
    {
        var errors = new List<string>();
        if (!request.Validate(ref errors))
        {
            Results.BadRequest(string.Join("\n", errors!));
        }    

        var distance = await gpsTriangulator.DistanceBetweenPoints(
            request.Point1.Latitude,
            request.Point1.Longitude,        
            request.Point2.Latitude,        
            request.Point2.Longitude);
        return Results.Ok(distance);
    }
}
