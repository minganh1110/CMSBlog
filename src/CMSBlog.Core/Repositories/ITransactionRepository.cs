using CMSBlog.Core.Domain.Royalty;
using CMSBlog.Core.Models;
using CMSBlog.Core.Models.Royalty;
using CMSBlog.Core.SeedWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction, Guid>
    {
        Task<PagedResult<TransactionDto>> GetAllPaging(string? userName,
         int fromMonth, int fromYear, int toMonth, int toYear, int pageIndex = 1, int pageSize = 10);
    }
}
