using System;
using System.Collections.Generic;
using System.Linq;

namespace CpuScheduling
{
    public static class Algorithms
    {
        public static (double avgWT, double avgTAT, double totalTime) FCFSAlgorithm(int np, List<Process> processes)
        {
            processes.Sort((a, b) => a.ArrivalTime.CompareTo(b.ArrivalTime));
            int currentTime = 0;
            double totalWT = 0, totalTAT = 0;

            foreach (var p in processes)
            {
                if (currentTime < p.ArrivalTime)
                    currentTime = p.ArrivalTime;

                int start = currentTime;
                int finish = currentTime + p.BurstTime;
                int turnaround = finish - p.ArrivalTime;
                int wait = start - p.ArrivalTime;

                totalWT += wait;
                totalTAT += turnaround;
                currentTime = finish;
            }

            return (totalWT / np, totalTAT / np, currentTime);
        }

        public static (double avgWT, double avgTAT, double totalTime) SJFAlgorithm(int np, List<Process> processes)
        {
            var remaining = new List<Process>(processes);
            int currentTime = 0;
            double totalWT = 0, totalTAT = 0;

            while (remaining.Count > 0)
            {
                var ready = remaining
                    .Where(p => p.ArrivalTime <= currentTime)
                    .OrderBy(p => p.BurstTime)
                    .ToList();

                if (ready.Count == 0)
                {
                    currentTime++;
                    continue;
                }

                var p = ready.First();
                int start = currentTime;
                int finish = currentTime + p.BurstTime;
                int turnaround = finish - p.ArrivalTime;
                int wait = start - p.ArrivalTime;

                totalWT += wait;
                totalTAT += turnaround;
                currentTime = finish;
                remaining.Remove(p);
            }

            return (totalWT / np, totalTAT / np, currentTime);
        }

        public static (double avgWT, double avgTAT, double totalTime) PriorityAlgorithm(int np, List<Process> processes)
        {
            var remaining = new List<Process>(processes);
            int currentTime = 0;
            double totalWT = 0, totalTAT = 0;

            while (remaining.Count > 0)
            {
                var ready = remaining
                    .Where(p => p.ArrivalTime <= currentTime)
                    .OrderBy(p => p.Priority)
                    .ToList();

                if (ready.Count == 0)
                {
                    currentTime++;
                    continue;
                }

                var p = ready.First();
                int start = currentTime;
                int finish = currentTime + p.BurstTime;
                int turnaround = finish - p.ArrivalTime;
                int wait = start - p.ArrivalTime;

                totalWT += wait;
                totalTAT += turnaround;
                currentTime = finish;
                remaining.Remove(p);
            }

            return (totalWT / np, totalTAT / np, currentTime);
        }

        public static (double avgWT, double avgTAT, double totalTime) RoundRobinAlgorithm(int np, List<Process> processes, int quantum)
        {
            double totalWT = 0, totalTAT = 0;
            double[] remaining = processes.Select(p => (double)p.BurstTime).ToArray();
            int[] arrival = processes.Select(p => p.ArrivalTime).ToArray();
            int[] burst = processes.Select(p => p.BurstTime).ToArray();
            int x = np, time = 0, counter = 0;
            double[] wait = new double[np];
            double[] turnaround = new double[np];

            while (x != 0)
            {
                for (int i = 0; i < np; i++)
                {
                    if (arrival[i] <= time && remaining[i] > 0)
                    {
                        if (remaining[i] <= quantum)
                        {
                            time += (int)remaining[i];
                            remaining[i] = 0;
                            counter = 1;
                        }
                        else
                        {
                            remaining[i] -= quantum;
                            time += quantum;
                        }

                        if (remaining[i] == 0 && counter == 1)
                        {
                            x--;
                            turnaround[i] = time - arrival[i];
                            wait[i] = turnaround[i] - burst[i];
                            totalWT += wait[i];
                            totalTAT += turnaround[i];
                            counter = 0;
                        }
                    }
                }

                if (!processes.Any(p => p.ArrivalTime <= time && remaining[processes.IndexOf(p)] > 0))
                    time++; // Idle time
            }

            return (totalWT / np, totalTAT / np, time);
        }

        // These are the two new implementations (Shortest Remaining Time First and Highest Response Ratio Next)
        public static (double avgWT, double avgTAT, double totalTime) SRTFAlgorithm(int np, List<Process> processes)
        {
            int[] rt = processes.Select(p => p.BurstTime).ToArray();
            int complete = 0, time = 0, minm = int.MaxValue;
            int shortest = 0, finish_time;
            bool check = false;
            double totalWT = 0, totalTAT = 0;

            // Keeps looping until all process are complete
            while (complete != np)
            {
                // Find process with shortest remaining time at current time
                for (int j = 0; j < np; j++)
                {
                    if ((processes[j].ArrivalTime <= time) && (rt[j] < minm) && rt[j] > 0)
                    {
                        minm = rt[j];
                        shortest = j;
                        check = true;
                    }
                }

                // If no process, increment time to simulate idle time
                if (!check)
                {
                    time++;
                    continue;
                }

                // Reduce remaining time of chosen process
                rt[shortest]--;
                minm = rt[shortest] == 0 ? int.MaxValue : rt[shortest];

                // If process is finished
                if (rt[shortest] == 0)
                {
                    complete++;
                    finish_time = time + 1;
                    int wt = finish_time - processes[shortest].BurstTime - processes[shortest].ArrivalTime;
                    int tat = finish_time - processes[shortest].ArrivalTime;
                    totalWT += wt;
                    totalTAT += tat;
                }
                time++;
            }

            return (totalWT / np, totalTAT / np, time);
        }

        public static (double avgWT, double avgTAT, double totalTime) HRRNAlgorithm(int np, List<Process> processes)
        {
            int completed = 0, time = 0;
            bool[] isCompleted = new bool[np];
            double totalWT = 0, totalTAT = 0;

            // Loop until all process are completed
            while (completed != np)
            {
                double hrr = double.MinValue; // Highest response ratio
                int index = -1;

                // Find the process with highest response ratio
                for (int i = 0; i < np; i++)
                {
                    if (processes[i].ArrivalTime <= time && !isCompleted[i])
                    {
                        double responseRatio = ((time - processes[i].ArrivalTime) + processes[i].BurstTime) / (double)processes[i].BurstTime;
                        if (responseRatio > hrr)
                        {
                            hrr = responseRatio;
                            index = i;
                        }
                    }
                }

                // If valid process is found
                if (index != -1)
                {
                    time += processes[index].BurstTime;
                    int tat = time - processes[index].ArrivalTime;
                    int wt = tat - processes[index].BurstTime;
                    totalWT += wt;
                    totalTAT += tat;
                    isCompleted[index] = true;
                    completed++;
                }
                else
                {
                    time++; // Idle time
                }
            }

            return (totalWT / np, totalTAT / np, time);
        }
    }
}