using Xunit;
using Moq;
using System.Collections.Generic;
using Testing_LR2;
using Testing_LR2.Objects;

namespace Testing_LR2_tests.FieldsAndMethodsTests;

public class ControllerTests
{
    private Controller controller;
    private Mock<SeismicSensor> seismicSensorMock;
    private Mock<DrillingEngineer> drillingEngineerMock;
    private Mock<OilSample> oilSampleMock;

    public ControllerTests()
    {
        controller = new Controller();
        seismicSensorMock = new Mock<SeismicSensor>();
        drillingEngineerMock = new Mock<DrillingEngineer>(new Mock<DrillingRig>().Object);
        oilSampleMock = new Mock<OilSample>();
    }

    [Fact]
    public void CheckState_InitialState_ReturnsOff()
    {
        // Act
        var state = controller.CheckState();

        // Assert
        Assert.Equal("Выключен", state);
    }

    [Fact]
    public void StartSystem_SetsStateToOn()
    {
        // Act
        controller.StartSystem();

        // Assert
        Assert.Equal("Включен", controller.CheckState());
    }

    [Fact]
    public void StopSystem_SetsStateToOff()
    {
        // Arrange
        controller.StartSystem();

        // Act
        controller.StopSystem();

        // Assert
        Assert.Equal("Выключен", controller.CheckState());
    }

    [Fact]
    public void CollectData_FromSeismicSensor_ReturnsData()
    {
        // Arrange
        var data = new Data("SeismicSensor", 50.0);
        seismicSensorMock.Setup(s => s.TransmitData(controller)).Returns(data);

        // Act
        var result = controller.CollectData(seismicSensorMock.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(data, result);
        seismicSensorMock.Verify(s => s.TransmitData(controller), Times.Once());
    }

    [Fact]
    public void SendCommand_ToDrillingEngineer_CallsReceiveCommand()
    {
        // Act
        controller.SendCommand(drillingEngineerMock.Object, "Запустить бурение");

        // Assert
        drillingEngineerMock.Verify(e => e.ReceiveCommand(controller, "Запустить бурение"), Times.Once());
    }

    [Fact]
    public void CheckDataSufficiency_ValidAnalysis_ReturnsTrue()
    {
        // Arrange
        var analysis = new Analysis("1", new Dictionary<string, double> { { "Нефть", 10.0 } });

        // Act
        var result = controller.CheckDataSufficiency(analysis);

        // Assert
        Assert.True(result);
    }
}