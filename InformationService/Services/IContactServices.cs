using InformationService.DTO;

namespace InformationService.Services
{
    public interface IContactServices
    {
        Task ContactFormReceived(ContactFormDTO contactFormDTO);
    }
}
