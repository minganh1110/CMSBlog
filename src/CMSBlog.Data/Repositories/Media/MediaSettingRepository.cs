using CMSBlog.Core.Application.DTOs.Media;
using CMSBlog.Core.Application.Interfaces.Media;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Data.Repositories.Media
{
    public class MediaSettingRepository : IMediaSettingRepository
    {
        private readonly CMSBlogContext _ctx;

        public MediaSettingRepository(CMSBlogContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<MediaSetting> GetAsync()
        {
            return await _ctx.MediaSettings.FirstAsync();
        }

        public async Task UpdateProviderAsync(string provider)
        {
            var setting = await _ctx.MediaSettings.FirstAsync();
            setting.ActiveProvider = provider;
            setting.UpdatedAt = DateTime.UtcNow;
            await _ctx.SaveChangesAsync();
        }
    }

}
