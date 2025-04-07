using Moq;
using Xunit;
using System;
using Testing_LR2.Objects;

namespace Testing_LR2_tests.FieldsAndMethodsTests;

public class SeismicSensorTests
{
    private SeismicSensor sensor;
    private Mock<Controller> controllerMock;

    public SeismicSensorTests()
    {
        sensor = new SeismicSensor();
        controllerMock = new Mock<Controller>();
    }

    [Fact]
    public void CheckState_InitialState_ReturnsStandby()
    {
        // Act
        var state = sensor.CheckState();

        // Assert
        Assert.Equal("В режиме ожидания", state);
    }

    [Fact]
    public void Activate_SetsStateToActive()
    {
        // Act
        sensor.Activate();

        // Assert
        Assert.Equal("Активен", sensor.CheckState());
    }

    [Fact]
    public void Deactivate_SetsStateToStandby()
    {
        // Arrange
        sensor.Activate();

        // Act
        sensor.Deactivate();

        // Assert
        Assert.Equal("В режиме ожидания", sensor.CheckState());
    }

    [Fact]
    public void RecordSeismicData_WhenActive_ReturnsData()
    {
        // Arrange
        sensor.Activate();

        // Act
        var data = sensor.RecordSeismicData();

        // Assert
        Assert.NotNull(data);
        Assert.True(data.Amplitude >= 0 && data.Amplitude <= 100);
    }

    [Fact]
    public void TransmitData_CallsController()
    {
        // Arrange
        sensor.Activate();
        sensor.RecordSeismicData();

        // Act
        var data = sensor.TransmitData(controllerMock.Object);

        // Assert
        Assert.NotNull(data);
    }
}