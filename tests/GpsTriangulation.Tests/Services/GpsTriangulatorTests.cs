namespace GpsTriangulation.Tests.Services;

public class GpsTriangulatorTests
{
    private readonly GpsTriangulator _sut;

    public GpsTriangulatorTests()
    {
        _sut = new GpsTriangulator();
    }

    [Fact]
    public async Task CalculateClosestStations_ShouldReturnMatchedPairs_WhenValidDataProvided()
    {
        // Arrange
        var baseData = new List<Dictionary<string, object>>
        {
            new() { { "id", 1 }, { "lat", 37.7749 }, { "lon", -122.4194 } },
            new() { { "id", 2 }, { "lat", 37.7750 }, { "lon", -122.4193 } }
        };

        var comparisonData = new List<Dictionary<string, object>>
        {
            new() { { "station_id", "STA-100" }, { "lat", 37.77491 }, { "lon", -122.41939 } },
            new() { { "station_id", "STA-200" }, { "lat", 37.7750 }, { "lon", -122.4195 } }
        };

        // Act
        var result = await _sut.CalculateClosestStations(
            baseData, comparisonData, "lat", "lon", "lat", "lon", 15.0);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].ClosestComparison.Should().NotBeNull();
        result[0].DistanceInFeet.Should().NotBeNull();
        result[0].DistanceInFeet.Should().BeLessThan(15.0);
    }

    [Fact]
    public async Task CalculateClosestStations_ShouldReturnNullClosest_WhenNoMatchWithinMaxDistance()
    {
        // Arrange
        var baseData = new List<Dictionary<string, object>>
        {
            new() { { "id", 1 }, { "lat", 37.7749 }, { "lon", -122.4194 } }
        };

        var comparisonData = new List<Dictionary<string, object>>
        {
            new() { { "station_id", "STA-100" }, { "lat", 37.8000 }, { "lon", -122.5000 } }
        };

        // Act
        var result = await _sut.CalculateClosestStations(
            baseData, comparisonData, "lat", "lon", "lat", "lon", 15.0);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].ClosestComparison.Should().BeNull();
        result[0].DistanceInFeet.Should().BeNull();
    }

    [Fact]
    public async Task DistanceBetweenPoints_ShouldReturnCorrectDistance()
    {
        // Arrange
        var lat1 = 37.7749;
        var lon1 = -122.4194;
        var lat2 = 37.77491;
        var lon2 = -122.41939;

        // Act
        var result = await _sut.DistanceBetweenPoints(lat1, lon1, lat2, lon2);

        // Assert
        result.Should().BeGreaterThan(0);
        result.Should().BeLessThan(5); // Should be very close, less than 5 feet
    }

    [Fact]
    public async Task DistanceBetweenPoints_ShouldReturnZero_WhenSamePoint()
    {
        // Arrange
        var lat = 37.7749;
        var lon = -122.4194;

        // Act
        var result = await _sut.DistanceBetweenPoints(lat, lon, lat, lon);

        // Assert
        result.Should().Be(0);
    }
}
