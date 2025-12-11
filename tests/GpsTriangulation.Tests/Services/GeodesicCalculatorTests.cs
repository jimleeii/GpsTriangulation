namespace GpsTriangulation.Tests.Services;

public class GeodesicCalculatorTests
{
    [Fact]
    public async Task CalculateVincenty_ShouldReturnCorrectDistance_BetweenKnownPoints()
    {
        // Arrange - San Francisco to Los Angeles (approximately 559 km)
        var point1 = new GeoPoint { Latitude = 37.7749, Longitude = -122.4194 };
        var point2 = new GeoPoint { Latitude = 34.0522, Longitude = -118.2437 };

        // Act
        var result = await GeodesicCalculator.CalculateVincenty(point1, point2);

        // Assert
        result.Should().NotBeNull();
        result.Distance.Should().BeGreaterThan(550000); // ~550 km in meters
        result.Distance.Should().BeLessThan(565000); // ~565 km in meters
        result.InitialBearing.Should().NotBe(0);
        result.FinalBearing.Should().NotBe(0);
    }

    [Fact]
    public async Task CalculateVincenty_ShouldReturnZero_ForSamePoint()
    {
        // Arrange
        var point1 = new GeoPoint { Latitude = 37.7749, Longitude = -122.4194 };
        var point2 = new GeoPoint { Latitude = 37.7749, Longitude = -122.4194 };

        // Act
        var result = await GeodesicCalculator.CalculateVincenty(point1, point2);

        // Assert
        result.Should().NotBeNull();
        result.Distance.Should().Be(0);
    }

    [Fact]
    public async Task CalculateVincenty_ShouldHandleShortDistances()
    {
        // Arrange - Two very close points (approximately 10 meters apart)
        var point1 = new GeoPoint { Latitude = 37.7749, Longitude = -122.4194 };
        var point2 = new GeoPoint { Latitude = 37.77491, Longitude = -122.41939 };

        // Act
        var result = await GeodesicCalculator.CalculateVincenty(point1, point2);

        // Assert
        result.Should().NotBeNull();
        result.Distance.Should().BeGreaterThan(0);
        result.Distance.Should().BeLessThan(2); // Less than 2 meters
    }

    [Fact]
    public async Task CalculateVincenty_ShouldHandleEquator()
    {
        // Arrange - Points on the equator
        var point1 = new GeoPoint { Latitude = 0, Longitude = 0 };
        var point2 = new GeoPoint { Latitude = 0, Longitude = 1 };

        // Act
        var result = await GeodesicCalculator.CalculateVincenty(point1, point2);

        // Assert
        result.Should().NotBeNull();
        result.Distance.Should().BeGreaterThan(110000); // ~111 km for 1 degree at equator
        result.Distance.Should().BeLessThan(112000);
    }
}
