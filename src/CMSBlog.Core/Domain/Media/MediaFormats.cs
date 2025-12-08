using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Domain.Media
{
    public class MediaFormats
    {
        public MediaFormat? Thumbnail { get; set; }
        public MediaFormat? Small { get; set; }
        public MediaFormat? Medium { get; set; }
        public MediaFormat? Large { get; set; }
    }

}
