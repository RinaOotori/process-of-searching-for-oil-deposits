using Xunit;
using System.Collections.Generic;
using Testing_LR2.Objects;

namespace Testing_LR2_tests.FieldsAndMethodsTests;

public class OilSampleTests
{
    private OilSample sample;

    public OilSampleTests()
    {
        sample = new OilSample();
    }

    [Fact]
    public void GetState_InitialState_ReturnsNotAnalyzed()
    {
        // Act
        var state = sample.GetState();

        // Assert
        Assert.Equal("Не исследован", state);
    }

    [Fact]
    public void StartAnalysis_SetsStateToInProgress()
    {
        // Act
        sample.StartAnalysis();

        // Assert
        Assert.Equal("В процессе анализа", sample.GetState());
    }

    [Fact]
    public void CompleteAnalysis_SetsStateToAnalyzed()
    {
        // Arrange
        var composition = new Dictionary<string, double> { { "Нефть", 50.0 } };
        sample.StartAnalysis();

        // Act
        sample.CompleteAnalysis(composition);

        // Assert
        Assert.Equal("Проанализирован", sample.GetState());
        Assert.Equal(composition, sample.GetComposition());
    }
}