namespace GpsTriangulation.Tests.DataContracts;

public class GpsTriangulatorDataRequestTests
{
    [Fact]
    public void Validate_ShouldReturnTrue_WhenAllDataIsValid()
    {
        // Arrange
        var request = new GpsTriangulatorDataRequest
        {
            BaseData = new List<Dictionary<string, object>>
            {
                new() { { "lat", 37.7749 }, { "lon", -122.4194 } }
            },
            ComparisonData = new List<Dictionary<string, object>>
            {
                new() { { "lat", 37.7750 }, { "lon", -122.4193 } }
            }
        };

        // Act
        List<string>? errors = null;
        var result = request.Validate(ref errors);

        // Assert
        result.Should().BeTrue();
        errors.Should().BeNullOrEmpty();
    }

    [Fact]
    public void Validate_ShouldReturnFalse_WhenBaseDataIsNull()
    {
        // Arrange
        var request = new GpsTriangulatorDataRequest
        {
            BaseData = null!,
            ComparisonData = new List<Dictionary<string, object>>()
        };

        // Act
        List<string>? errors = null;
        var result = request.Validate(ref errors);

        // Assert
        result.Should().BeFalse();
        errors.Should().NotBeNull();
        errors.Should().Contain(e => e.Contains("BaseData"));
    }

    [Fact]
    public void Validate_ShouldReturnFalse_WhenComparisonDataIsNull()
    {
        // Arrange
        var request = new GpsTriangulatorDataRequest
        {
            BaseData = new List<Dictionary<string, object>>(),
            ComparisonData = null!
        };

        // Act
        List<string>? errors = null;
        var result = request.Validate(ref errors);

        // Assert
        result.Should().BeFalse();
        errors.Should().NotBeNull();
        errors.Should().Contain(e => e.Contains("ComparisonData"));
    }

    [Fact]
    public void Constructor_ShouldSetDefaultValues()
    {
        // Arrange & Act
        var request = new GpsTriangulatorDataRequest
        {
            BaseData = new List<Dictionary<string, object>>(),
            ComparisonData = new List<Dictionary<string, object>>()
        };

        // Assert
        request.BaseLatColumn.Should().Be("lat");
        request.BaseLonColumn.Should().Be("lon");
        request.TargetLatColumn.Should().Be("lat");
        request.TargetLonColumn.Should().Be("lon");
        request.MaxDistance.Should().Be(15);
    }
}
