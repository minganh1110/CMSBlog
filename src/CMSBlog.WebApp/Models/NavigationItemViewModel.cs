namespace CMSBlog.WebApp.Models
{
    public class NavigationItemViewModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public bool OpenInNewTab { get; set; }

        public List<NavigationItemViewModel> Children { get; set; } = new List<NavigationItemViewModel>();

        public bool HasChildren
        {
            get
            {
                return Children.Count > 0;
            }
        }
    }
}
