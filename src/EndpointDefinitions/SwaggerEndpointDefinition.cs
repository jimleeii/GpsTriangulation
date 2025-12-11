using EndpointDefinition;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace GpsTriangulation.EndpointDefinitions;

/// <summary>
/// The swagger endpoint definition.
/// </summary>
public class SwaggerEndpointDefinition : IEndpointDefinition
{
    private readonly string Title = Assembly.GetEntryAssembly()!.GetName().Name!;
    private const string Version = "v1";
    private ILogger<SwaggerEndpointDefinition>? Logger;

    /// <summary>
    /// Defines the endpoints.
    /// </summary>
    /// <param name="app">The app.</param>
    /// <param name="env">The environment.</param>
    public void DefineEndpoints(WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            Logger!.LogInformation("Using Swagger UI");
            app.UseSwagger();
            // Enable OpenAPI endpoint
		    app.UseSwaggerUI(c => c.SwaggerEndpoint("/openapi/v1.json", $"{Title} {Version}"));
        }
    }

    /// <summary>
    /// Defines the services.
    /// </summary>
    /// <param name="services">The services.</param>
    public void DefineServices(IServiceCollection services)
    {
        Logger = services.BuildServiceProvider().GetRequiredService<ILogger<SwaggerEndpointDefinition>>();

        services.AddEndpointsApiExplorer();
        // Add OpenAPI support
		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc(Version, new OpenApiInfo { Title = Title, Version = Version });
		});
    }
}