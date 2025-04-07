using System;
using System.Collections.Generic;

namespace Testing_LR2;
// Тип для сейсмических данных
public class SeismicData
{
    public double Amplitude { get; set; }
    public DateTime Timestamp { get; set; }

    public SeismicData(double amplitude)
    {
        Amplitude = amplitude;
        Timestamp = DateTime.Now;
    }
}

// Тип для данных, передаваемых между объектами
public class Data
{
    public string Source { get; set; }
    public object Value { get; set; }

    public Data(string source, object value)
    {
        Source = source;
        Value = value;
    }
}

// Тип для обработанных данных
public class ProcessedData
{
    public List<Data> RawData { get; set; }
    public Dictionary<string, double> ProcessedValues { get; set; }

    public ProcessedData(List<Data> rawData)
    {
        RawData = rawData;
        ProcessedValues = new Dictionary<string, double>();
    }
}

// Тип для анализа образца
public class Analysis
{
    public string SampleId { get; set; }
    public Dictionary<string, double> Composition { get; set; }

    public Analysis(string sampleId, Dictionary<string, double> composition)
    {
        SampleId = sampleId;
        Composition = composition;
    }
}