using CMSBlog.Core.Application.Mapping;

namespace CMSBlog.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // <summary>
        // Đăng ký AutoMapper và các dịch vụ ứng dụng khác
        // </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MediaProfile).Assembly);
            return services;
        }
    }
}
