using CMSBlog.Core.Domain.Info;

namespace CMSBlog.Core.Services
{
    public interface IFooterService
    {
        Task<FooterSettings?> GetActiveSettingsAsync();
        Task<List<FooterLink>> GetActiveLinksAsync();
    }
}

