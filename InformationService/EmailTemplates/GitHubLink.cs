using System.Text;

namespace InformationService.EmailTemplates;

public partial class EmailTemplate
{
    public static string GitHubLink(string receiverName, string repoUrl, string senderEmailAddress)
    {
        var Body = new StringBuilder();
        Body.Append(@$"
<p> Goedendag {receiverName},</p>
<p> Bedankt voor uw interesse, hier is de link naar mijn GitHub Repository</p>
<p>
    <a href="" {repoUrl}
        "" >{repoUrl}</a>
</p>
<p> Als u nog vragen heeft mag u me altijd mailen </p>
<p>
    Mvg,<br/>
    Brecht Spincemaille <br/>
    {senderEmailAddress}
</p>
");
        return Body.ToString();
    }
}