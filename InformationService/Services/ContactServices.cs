using InformationService.Data.Repositories;
using InformationService.DTO;
using InformationService.Models;

namespace InformationService.Services
{
    public class ContactServices(
        ContactContext contactContext) : IContactServices
    {
        private readonly ContactContext _contactContext = contactContext;
        public async Task ContactFormReceived(ContactFormDTO contactFormDTO)
        {
            var newContact = new Contact
            {
                Name = contactFormDTO.Name,
                Email = contactFormDTO.Email,
            };

            _contactContext.Contacts.Add(newContact);
            await _contactContext.SaveChangesAsync();
            //TODO get link to user
        }
    }
}
