
public class MetricResult
{
    public string Name { get; set; }
    public double AvgWT { get; set; }
    public double AvgTAT { get; set; }
    public double CpuUtil { get; set; }
    public double Throughput { get; set; }

    public MetricResult(string name, double avgWT, double avgTAT, double cpuUtil, double throughput)
    {
        Name = name;
        AvgWT = avgWT;
        AvgTAT = avgTAT;
        CpuUtil = cpuUtil;
        Throughput = throughput;
    }

    // Print all data in a row
    public void PrintRow()
    {
        Console.WriteLine(string.Format("{0,-12} | {1,8:F2} | {2,8:F2} | {3,8:F2}% | {4,10:F2}/sec",
            Name, AvgWT, AvgTAT, CpuUtil, Throughput));
    }
}

public static class MetricTable
{
    public static void PrintHeader(string testName)
    {
        Console.WriteLine(); // new line
        Console.WriteLine($"--- {testName} Results ---");
        Console.WriteLine(string.Format("{0,-12} | {1,8} | {2,8} | {3,8} | {4,12}",
            "Algorithm", "Avg WT", "Avg TAT", "CPU Util", "Throughput"));
        Console.WriteLine(new string('-', 60));
    }

    public static void Print(List<MetricResult> results, string testName)
    {
        PrintHeader(testName);
        foreach (var result in results)
            result.PrintRow();
        Console.WriteLine();
    }
}
