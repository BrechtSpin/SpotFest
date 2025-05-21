namespace InformationService.Models;

public class EmailSettings
{
    public string SmtpServer { get; set; } = "";
    public int SmtpPort { get; set; } = 587;
    public string FromAddress { get; set; } = "";
}

