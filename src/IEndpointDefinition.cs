namespace GpsTriangulation;

/// <summary>
/// Interface for defining endpoint services and endpoints.
/// </summary>
public interface IEndpointDefinition
{
    /// <summary>
    /// Defines the services required by the endpoint.
    /// </summary>
    /// <param name="services">The service collection to which the services are added.</param>
    void DefineServices(IServiceCollection services);

    /// <summary>
    /// Defines the endpoints for the application.
    /// </summary>
    /// <param name="app">The web application to which the endpoints are added.</param>
    /// <param name="evnt">The hosting environment for the application.</param>
    void DefineEndpoints(WebApplication app, IWebHostEnvironment evnt);
}
