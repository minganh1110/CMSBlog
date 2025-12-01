using CMSBlog.Core.Application.Interfaces.Media;

namespace CMSBlog.API.Services
{
    public class StorageFactory : IStorageFactory
    {
        private readonly IEnumerable<IStorageServicee> _providers;
        private readonly IMediaSettingRepository _settingRepo;

        public StorageFactory(IEnumerable<IStorageServicee> providers, IMediaSettingRepository settingRepo)
        {
            _providers = providers;
            _settingRepo = settingRepo;
        }

        public async Task<IStorageServicee> GetStorageServiceAsync()
        {
            // Lấy setting từ DB
            var setting = await _settingRepo.GetAsync();
            var providerName = setting?.ActiveProvider;

            if (string.IsNullOrEmpty(providerName))
                throw new Exception("Active storage provider is not configured in database.");

            // Tìm provider theo ProviderName
            var service = _providers.FirstOrDefault(x => x.ProviderName == providerName);

            if (service == null)
                throw new Exception($"Unknown storage provider: {providerName}" +
                    $"x.ProviderName: {_providers.FirstOrDefault(x => x.ProviderName == "S3")}");
            

            return service;
        }
    }




}
