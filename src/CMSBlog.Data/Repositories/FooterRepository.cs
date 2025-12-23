using System;
using CMSBlog.Core.Domain.Info;
using CMSBlog.Core.Repositories;
using CMSBlog.Data.SeedWorks;
using Microsoft.EntityFrameworkCore;

namespace CMSBlog.Data.Repositories
{
    public class FooterRepository : RepositoryBase<FooterSettings, Guid>, IFooterRepository
    {
        public FooterRepository(CMSBlogContext context) : base(context)
        {
        }

        public async Task<FooterSettings?> GetActiveSettingsAsync()
        {
            return await _context.FooterSettings
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.DateCreated)
                .FirstOrDefaultAsync();
        }

        public async Task<List<FooterLink>> GetActiveLinksAsync()
        {
            return await _context.FooterLinks
                .Where(x => x.IsActive)
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.DateCreated)
                .ToListAsync();
        }

        public Task<FooterLink?> GetLinkByIdAsync(Guid id)
        {
            return _context.FooterLinks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void AddLink(FooterLink link)
        {
            _context.FooterLinks.Add(link);
        }

        public void RemoveLink(FooterLink link)
        {
            _context.FooterLinks.Remove(link);
        }
    }
}

