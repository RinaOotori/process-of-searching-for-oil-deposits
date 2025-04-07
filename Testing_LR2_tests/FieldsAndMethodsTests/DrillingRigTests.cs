using Testing_LR2.Objects;
using Xunit;

namespace Testing_LR2_tests.FieldsAndMethodsTests;

public class DrillingRigTests
{
    private DrillingRig rig;

    public DrillingRigTests()
    {
        rig = new DrillingRig();
    }

    [Fact]
    public void CheckState_InitialState_ReturnsStopped()
    {
        // Act
        var state = rig.CheckState();

        // Assert
        Assert.Equal("Остановлена", state);
    }

    [Fact]
    public void StartDrilling_SetsStateToWorking()
    {
        // Act
        rig.StartDrilling();

        // Assert
        Assert.Equal("Работает", rig.CheckState());
    }

    [Fact]
    public void StopDrilling_SetsStateToStopped()
    {
        // Arrange
        rig.StartDrilling();

        // Act
        rig.StopDrilling();

        // Assert
        Assert.Equal("Остановлена", rig.CheckState());
    }

    [Fact]
    public void ExtractSample_WhenWorking_ReturnsSample()
    {
        // Arrange
        rig.StartDrilling();

        // Act
        var sample = rig.ExtractSample();

        // Assert
        Assert.NotNull(sample);
    }

    [Fact]
    public void PerformMaintenance_WhenNeedsRepair_SetsStateToStopped()
    {
        // Arrange
        rig.StartDrilling();
        // Имитация состояния "Требует ремонта" (в реальном коде это может быть установлено через другой метод)
        typeof(DrillingRig).GetField("state", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(rig, "Требует ремонта");

        // Act
        rig.PerformMaintenance();

        // Assert
        Assert.Equal("Остановлена", rig.CheckState());
    }
}