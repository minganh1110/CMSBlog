using CMSBlog.Core.Application.Interfaces.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Application.Services.Media
{
    public class ProviderService : IProviderService
    {
        private readonly IMediaSettingRepository _repo;

        public ProviderService(IMediaSettingRepository repo)
        {
            _repo = repo;
        }

        public async Task ChangeProviderAsync(string provider)
        {
            var allowed = new[] { "Local", "S3" };

            if (!allowed.Contains(provider))
                throw new Exception("Provider không hợp lệ");

            await _repo.UpdateProviderAsync(provider);
        }

        public async Task<string> GetCurrentProviderAsync()
        {
            return (await _repo.GetAsync()).ActiveProvider;
        }
    }

}
