using CMSBlog.Core.Application.DTOs.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Application.Interfaces.Media
{
    public interface IMediaSettingRepository
    {
        Task<MediaSetting> GetAsync();
        Task UpdateProviderAsync(string provider);
    }

}
