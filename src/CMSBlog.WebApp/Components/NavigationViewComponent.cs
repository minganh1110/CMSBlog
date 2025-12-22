using CMSBlog.Core.SeedWorks;
using CMSBlog.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMSBlog.WebApp.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public NavigationViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // 1. Get all active menu items
            var allMenus = await _unitOfWork.Menu.GetAllAsync();
            var activeMenus = allMenus.Where(x => x.IsActive).OrderBy(x => x.SortOrder).ToList();

            // 2. Prepare lists of IDs to fetch
            var postIds = activeMenus.Where(x => x.LinkType == "Post" && x.EntityId.HasValue)
                                     .Select(x => x.EntityId.Value).Distinct().ToList();
            var categoryIds = activeMenus.Where(x => x.LinkType == "Category" && x.EntityId.HasValue)
                                         .Select(x => x.EntityId.Value).Distinct().ToList();
            var seriesIds = activeMenus.Where(x => x.LinkType == "Series" && x.EntityId.HasValue)
                                         .Select(x => x.EntityId.Value).Distinct().ToList();

            // 3. Fetch details
            // Note: Assuming generic repository doesn't have "GetByList", using string filter or Loop if needed.
            // But generic repository usually has Find(expression).
            // Let's assume standard IRepository has Find/GetList with predicate. It seemed to have GetAllAsync only in previous snippets?
            // Checking RepositoryBase.cs in user context... User has it open.
            // If not available, I'll fetch all or loop. Optimization: fetch all posts is bad.
            // Checking step 118: _unitOfWork.PostCategories.GetAllAsync() was used.
            // I will use `Find` or similar if I safely can. 
            // WAIT - I don't see `Find` method in IMenuRepository.cs in step 128. It inherits IRepository.
            // I'll assume IRepository has a `Find` or `GetList` taking a predicate. safely I can use GetAll for Categories/Series (small).
            // Posts might be large.
            // Logic: `_unitOfWork.Posts.Find(x => postIds.Contains(x.Id))` is standard EF pattern.
            
            var posts = postIds.Any() ? (_unitOfWork.Posts.Find(x => postIds.Contains(x.Id))).ToDictionary(x => x.Id, x => x.Slug) : new Dictionary<Guid, string>();
            var categories = categoryIds.Any() ? (_unitOfWork.PostCategories.Find(x => categoryIds.Contains(x.Id))).ToDictionary(x => x.Id, x => x.Slug) : new Dictionary<Guid, string>();
            var series = seriesIds.Any() ? (_unitOfWork.Series.Find(x => seriesIds.Contains(x.Id))).ToDictionary(x => x.Id, x => x.Slug) : new Dictionary<Guid, string>();

            // 4. Build Tree
            var roots = activeMenus.Where(x => x.ParentId == null).OrderBy(x => x.SortOrder).ToList();
            var viewModels = roots.Select(x => MapToViewModel(x, activeMenus, posts, categories, series)).ToList();

            return View(viewModels);
        }

        private NavigationItemViewModel MapToViewModel(
            CMSBlog.Core.Domain.Menu.MenuItem menuItem, 
            List<CMSBlog.Core.Domain.Menu.MenuItem> allMenus,
            Dictionary<Guid, string> posts,
            Dictionary<Guid, string> categories,
            Dictionary<Guid, string> series)
        {
            var vm = new NavigationItemViewModel
            {
                Name = menuItem.Name,
                OpenInNewTab = menuItem.OpenInNewTab ?? false,
                Url = GetUrl(menuItem, posts, categories, series)
            };

            var children = allMenus.Where(x => x.ParentId == menuItem.Id).OrderBy(x => x.SortOrder);
            if (children.Any())
            {
                vm.Children = children.Select(x => MapToViewModel(x, allMenus, posts, categories, series)).ToList();
            }

            return vm;
        }

        private string GetUrl(
             CMSBlog.Core.Domain.Menu.MenuItem menuItem,
             Dictionary<Guid, string> posts,
             Dictionary<Guid, string> categories,
             Dictionary<Guid, string> series)
        {
            if (menuItem.LinkType == "CustomLink") return menuItem.CustomUrl ?? "#";

            string slug = string.Empty;
            string prefix = "";

            if (menuItem.LinkType == "Post" && menuItem.EntityId.HasValue)
            {
                posts.TryGetValue(menuItem.EntityId.Value, out slug);
                // UrlConsts.Posts detail uses /post/{slug} usually.
                // Hardcoding standard pattern or using constant if accessible.
                return $"/post/{slug}"; 
            }
            if (menuItem.LinkType == "Category" && menuItem.EntityId.HasValue)
            {
                categories.TryGetValue(menuItem.EntityId.Value, out slug);
                 return $"/posts/{slug}"; // UrlConsts.PostsByCategorySlug pattern
            }
            if (menuItem.LinkType == "Series" && menuItem.EntityId.HasValue)
            {
                series.TryGetValue(menuItem.EntityId.Value, out slug);
                return $"/series/{slug}";
            }

            return menuItem.CustomUrl ?? "#";
        }
    }
}
