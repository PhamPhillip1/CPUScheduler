public class Process
{
    public string Id;
    public int ArrivalTime;
    public int BurstTime;
    public int Priority;
    public int RemainingTime;

    public Process(string id, int arrival, int burst, int priority)
    {
        Id = id;
        ArrivalTime = arrival;
        BurstTime = burst;
        Priority = priority;
        RemainingTime = burst;
    }

    public Process Clone()
    {
        return new Process(Id, ArrivalTime, BurstTime, Priority);
    }
}