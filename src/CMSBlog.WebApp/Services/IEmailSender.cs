using CMSBlog.WebApp.Models;

namespace CMSBlog.WebApp.Services
{
    public interface IEmailSender
    {
        Task SendEmail(EmailData emailData);
    }
}
