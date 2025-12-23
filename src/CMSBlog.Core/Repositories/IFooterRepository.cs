using CMSBlog.Core.Domain.Info;
using CMSBlog.Core.SeedWorks;

namespace CMSBlog.Core.Repositories
{
    public interface IFooterRepository : IRepository<FooterSettings, Guid>
    {
        Task<FooterSettings?> GetActiveSettingsAsync();
        Task<List<FooterLink>> GetActiveLinksAsync();
        Task<FooterLink?> GetLinkByIdAsync(Guid id);
        void AddLink(FooterLink link);
        void RemoveLink(FooterLink link);
    }
}

