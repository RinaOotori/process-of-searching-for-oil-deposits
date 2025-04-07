using Testing_LR2.Objects;
using Xunit;

namespace Testing_LR2_tests.FieldsAndMethodsTests;

public class MapTests
{
    private Map map;

    public MapTests()
    {
        map = new Map();
    }

    [Fact]
    public void GetState_InitialState_ReturnsNotCreated()
    {
        // Act
        var state = map.GetState();

        // Assert
        Assert.Equal("Не создана", state);
    }

    [Fact]
    public void StartCreation_SetsStateToInProgress()
    {
        // Act
        map.StartCreation();

        // Assert
        Assert.Equal("В процессе построения", map.GetState());
    }

    [Fact]
    public void CompleteCreation_SetsStateToCompleted()
    {
        // Arrange
        map.StartCreation();

        // Act
        map.CompleteCreation();

        // Assert
        Assert.Equal("Завершена", map.GetState());
    }
}