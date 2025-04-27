# Operating Systems Project 2: CPU Scheduling Simulator

## Overview
A CPU Scheduling Simulator created in a Operating Systems course and developed in C# for experimental purposes. It supports multiple scheduling algorithms and computes performance metrics like average waiting time, average turnaround time, CPU utilization, and throughput.

---

## Features
### Mutliple Scheduling Algorithms:
- First Come, First Served (FCFS)
- Shortest Job First (SJF)
- Priority Scheduling
- Round Robin
- Shortest Remaining Time First (SRTF)
- Highest Response Ratio Next (HRRN)

### Performance Metrics:
- Average Waiting Time
- Average Turnaround Time
- CPU Utilization
- Throughput

--- 

## Technologies Used
- Language: C#
- Framework: .NET SDK (Version 9.0.203)
- IDE: Visual Studio Code

---

## Getting Started
Make sure .NET SDK and visual studio code is downloaded. 


[Link to download .NET SDK](https://dotnet.microsoft.com/en-us/download)

[Link to download VS Code](https://code.visualstudio.com/Download)

Once downloaded:

Check if .NET SDK was successfully downloaded by typing this in the terminal:

- dotnet --info

Download the following extensions for C# in VS Code:

- C#
- C# Dev Kit
- Code Runner
- CMake and CMake Tools
- .NET Install Tool

To Run Simulation: 

1. Clone the GitHub repository by typing this in the terminal:

```bash
git clone https://github.com/PhamPhillip1/CPUScheduler.git
```

2. Navigate to the project directory by typing in the terminal:
- cd CPUScheduler

3. Build the application by typing this in the terminal:
- dotnet build

4. Run the application by typing this in the terminal:
- dotnet run

Congrats! The program should run now. If you want to change values to see different results, go to Program.cs and change the values of the processes.


