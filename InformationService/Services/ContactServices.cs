﻿using InformationService.Data.Repositories;
using InformationService.DTO;
using InformationService.Models;
using InformationService.EmailTemplates;
using System.Net;
using System.Diagnostics;

namespace InformationService.Services;

public class ContactServices(
    ILogger<ContactServices> _logger,
    ContactContext contactContext,
    IEmailService emailService,
    EmailSettings emailSettings) : IContactServices
{
    private readonly ContactContext _contactContext = contactContext;
    private readonly IEmailService _emailService = emailService;
    private readonly EmailSettings _emailSettings = emailSettings;
    public async Task ContactFormReceived(ContactFormDTO contactFormDTO)
    {
        var newContact = new Contact
        {
            Name = contactFormDTO.Name,
            Email = contactFormDTO.Email,
        };

        try
        {
            _contactContext.Contacts.Add(newContact);
            await _contactContext.SaveChangesAsync();

            var sanName = WebUtility.HtmlEncode(newContact.Name);
            var sanEmail = WebUtility.HtmlEncode(newContact.Email);

            var emailBody = EmailTemplate.GitHubLink(
                sanName,
                "https://github.com/BrechtSpin/SpotFest",
                _emailSettings.ReplyToEmail);

            await _emailService.SendEmailAsync(
                sanEmail,
                "Your Github repository link",
                emailBody,
                _emailSettings.ReplyToEmail);
            await _emailService.SendEmailAsync(
                _emailSettings.ReplyToEmail,
                "A Github link was sent",
                WebUtility.HtmlEncode($"A Github link was sent to {sanName} {sanEmail}"),
                _emailSettings.ReplyToEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
}
