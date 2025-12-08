using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Domain.Media
{
    public class MediaFormat
    {
        public string Name { get; set; } = null!;
        public string Hash { get; set; } = null!;
        public string Ext { get; set; } = null!;
        public string Mime { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string? Path { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public double Size { get; set; }
        public long SizeInBytes { get; set; }
    }

}
