namespace TaskScheduler.Models;

public class TaskSchedulerLog
{
    public Guid Guid { get; set; } =  Guid.NewGuid();  
    public required DateTime TaskDate { get; set; }
}
