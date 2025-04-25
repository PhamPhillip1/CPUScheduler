
using System;
using System.Collections.Generic;
using System.Linq;
using CpuScheduling;

namespace CPUScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAllTests();
        }

        // Runs the basic test, large scale test, and edge test
        static void RunAllTests()
        {
            Console.WriteLine("=== BASIC TEST ===");
            var basicTest = new List<Process>
            {
                new Process("P1", 0, 8, 2),
                new Process("P2", 1, 4, 1),
                new Process("P3", 2, 9, 3),
                new Process("P4", 3, 5, 2),
                new Process("P5", 4, 2, 1)
            };
            RunAllAlgorithms(basicTest, "Basic Test");

            Console.WriteLine("\n=== LARGE SCALE TEST ===");
            var largeTest = GenerateRandomProcesses(50);
            RunAllAlgorithms(largeTest, "Large Scale Test");

            Console.WriteLine("\n=== EDGE CASE TEST ===");
            var edgeTest = new List<Process>
            {
                new Process("P1", 0, 20, 3),
                new Process("P2", 0, 20, 1),
                new Process("P3", 0, 20, 2),
                new Process("P4", 0, 20, 5),
                new Process("P5", 0, 20, 4)
            };
            RunAllAlgorithms(edgeTest, "Edge Case Test");

        }
        
        // Method to run the six algorithms for each test
        static void RunAllAlgorithms(List<Process> originalList, string testName)
        {
            int np = originalList.Count;
            var results = new List<MetricResult>();

            results.Add(RunWithMetrics("FCFS", () => Algorithms.FCFSAlgorithm(np, Clone(originalList)), originalList));
            results.Add(RunWithMetrics("SJF", () => Algorithms.SJFAlgorithm(np, Clone(originalList)), originalList));
            results.Add(RunWithMetrics("Priority", () => Algorithms.PriorityAlgorithm(np, Clone(originalList)), originalList));
            results.Add(RunWithMetrics("Round Robin", () => Algorithms.RoundRobinAlgorithm(np, Clone(originalList), 2), originalList));
            results.Add(RunWithMetrics("SRTF", () => Algorithms.SRTFAlgorithm(np, Clone(originalList)), originalList));
            results.Add(RunWithMetrics("HRRN", () => Algorithms.HRRNAlgorithm(np, Clone(originalList)), originalList));

            PrintMetricTable(results, testName);
        }

        // Method to get the CPU utilization and throughput and prints it
        static MetricResult RunWithMetrics(string name, Func<(double avgWT, double avgTAT, double simTime)> algorithm, List<Process> processes)
        {
          
            Console.WriteLine($"\n--- {name} ---");

            double totalBurstTime = processes.Sum(p => p.BurstTime);

            var (avgWT, avgTAT, simTime) = algorithm();

            double cpuUtil = (totalBurstTime / simTime) * 100;
            double throughput = processes.Count / simTime;

            Console.WriteLine($"[Metrics] CPU Utilization: {cpuUtil:F2}%");
            Console.WriteLine($"[Metrics] Throughput: {throughput:F2} processes/second");

            return new MetricResult(name, avgWT, avgTAT, cpuUtil, throughput);
        }
        
        static List<Process> Clone(List<Process> processes)
        {
            var newList = new List<Process>();
            foreach (var p in processes)
                newList.Add(p.Clone());
            return newList;
        }

        // Generate random arrival, brust, and priority processes for large scale test
        static List<Process> GenerateRandomProcesses(int count)
        {
            var rand = new Random();
            var list = new List<Process>();
            for (int i = 1; i <= count; i++)
            {
                string pid = $"P{i}";
                int arrival = rand.Next(0, 100);
                int burst = rand.Next(5, 15);
                int priority = rand.Next(1, 10);
                list.Add(new Process(pid, arrival, burst, priority));
            }
            return list;
        }

        // Prints the key metrics in a organized and clean table
        static void PrintMetricTable(List<MetricResult> results, string testName)
        {
            Console.WriteLine();
            Console.WriteLine($"--- {testName} Results ---");
            Console.WriteLine(string.Format("{0,-12} | {1,8} | {2,8} | {3,8} | {4,12}", "Algorithm", "Avg WT", "Avg TAT", "CPU Util", "Throughput"));
            Console.WriteLine(new string('-', 60));
            foreach (var result in results)
            {
                Console.WriteLine(string.Format("{0,-12} | {1,8:F2} | {2,8:F2} | {3,8:F2}% | {4,10:F2}/sec",
                    result.Name, result.AvgWT, result.AvgTAT, result.CpuUtil, result.Throughput));
            }
            Console.WriteLine();
        }

        public class MetricResult
        {
            public string Name;
            public double AvgWT;
            public double AvgTAT;
            public double CpuUtil;
            public double Throughput;

            public MetricResult(string name, double avgWT, double avgTAT, double cpuUtil, double throughput)
            {
                Name = name;
                AvgWT = avgWT;
                AvgTAT = avgTAT;
                CpuUtil = cpuUtil;
                Throughput = throughput;
            }
        }
    }
}
