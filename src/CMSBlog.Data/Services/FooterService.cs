using System.Collections.Generic;
using System.Threading.Tasks;
using CMSBlog.Core.Domain.Info;
using CMSBlog.Core.Repositories;
using CMSBlog.Core.Services;

namespace CMSBlog.Data.Services
{
    public class FooterService : IFooterService
    {
        private readonly IFooterRepository _footerRepository;

        public FooterService(IFooterRepository footerRepository)
        {
            _footerRepository = footerRepository;
        }

        public Task<FooterSettings?> GetActiveSettingsAsync()
        {
            return _footerRepository.GetActiveSettingsAsync();
        }

        public Task<List<FooterLink>> GetActiveLinksAsync()
        {
            return _footerRepository.GetActiveLinksAsync();
        }
    }
}

