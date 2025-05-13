namespace JobScheduler.Models;

public class JobSchedulerLog
{
    public Guid Guid { get; set; } =  Guid.NewGuid();  
    public required DateTime JobDate { get; set; }
}
