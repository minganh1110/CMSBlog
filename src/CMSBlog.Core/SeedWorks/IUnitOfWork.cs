


using CMSBlog.Core.Repositories;

namespace CMSBlog.Core.SeedWorks
{
    public interface IUnitOfWork
    {
        IPostRepository Posts { get; }

        Task<int> CompleteAsync();
    }
}
