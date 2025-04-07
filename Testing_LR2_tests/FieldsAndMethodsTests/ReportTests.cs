using Testing_LR2.Objects;
using Xunit;

namespace Testing_LR2_tests.FieldsAndMethodsTests;

public class ReportTests
{
    private Report report;

    public ReportTests()
    {
        report = new Report();
    }

    [Fact]
    public void GetState_InitialState_ReturnsNotStarted()
    {
        // Act
        var state = report.GetState();

        // Assert
        Assert.Equal("Не начат", state);
    }

    [Fact]
    public void StartCompilation_SetsStateToInProgress()
    {
        // Act
        report.StartCompilation();

        // Assert
        Assert.Equal("В процессе составления", report.GetState());
    }

    [Fact]
    public void CompleteCompilation_SetsStateToCompleted()
    {
        // Arrange
        report.StartCompilation();

        // Act
        report.CompleteCompilation();

        // Assert
        Assert.Equal("Завершен", report.GetState());
    }

    [Fact]
    public void GetReserveEstimate_AfterCompletion_ReturnsEstimate()
    {
        // Arrange
        report.StartCompilation();
        report.SetReserveEstimate(1000.0);
        report.CompleteCompilation();

        // Act
        var estimate = report.GetReserveEstimate();

        // Assert
        Assert.Equal(1000.0, estimate);
    }
}