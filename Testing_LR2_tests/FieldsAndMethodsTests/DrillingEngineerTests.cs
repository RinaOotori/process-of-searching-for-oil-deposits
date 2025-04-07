using Moq;
using Testing_LR2.Objects;
using Xunit;

namespace Testing_LR2_tests.FieldsAndMethodsTests;

public class DrillingEngineerTests
{
    private DrillingEngineer engineer;
    private Mock<DrillingRig> drillingRigMock;
    private Mock<Controller> controllerMock;

    public DrillingEngineerTests()
    {
        drillingRigMock = new Mock<DrillingRig>();
        engineer = new DrillingEngineer(drillingRigMock.Object);
        controllerMock = new Mock<Controller>();
    }

    [Fact]
    public void GetState_InitialState_ReturnsResting()
    {
        // Act
        var state = engineer.GetState();

        // Assert
        Assert.Equal("Отдыхает", state);
    }

    [Fact]
    public void StartWork_SetsStateToWorking()
    {
        // Act
        engineer.StartWork();

        // Assert
        Assert.Equal("Работает", engineer.GetState());
    }

    [Fact]
    public void ReceiveCommand_StartDrilling_CallsStartDrilling()
    {
        // Arrange
        engineer.StartWork();

        // Act
        engineer.ReceiveCommand(controllerMock.Object, "Запустить бурение");

        // Assert
        drillingRigMock.Verify(r => r.StartDrilling(), Times.Once());
    }
}