


using CMSBlog.Core.Repositories;

namespace CMSBlog.Core.SeedWorks
{
    public interface IUnitOfWork
    {
        IPostRepository Posts { get; }
        IPostCategoryRepository PostCategories { get; }
        ISeriesRepository Series { get; }
        ITransactionRepository Transactions { get; }
        Task<int> CompleteAsync();
    }
}
