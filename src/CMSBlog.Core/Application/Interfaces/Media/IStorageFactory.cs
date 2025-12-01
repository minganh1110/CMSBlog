using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Application.Interfaces.Media
{
    public interface IStorageFactory
    {
        Task<IStorageServicee> GetStorageServiceAsync();
    }

}
