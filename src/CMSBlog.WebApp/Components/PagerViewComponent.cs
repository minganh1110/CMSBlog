using CMSBlog.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMSBlog.WebApp.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagedResultBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
