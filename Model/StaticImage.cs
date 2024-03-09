using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model
{
    public class StaticImage
    {
        public string base64image { get; set; }
        public string imagecontext { get; set; }
        public string contenttype { get; set; }

        public string imagename { get; set; }
        public string imagepath { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string alt { get; set; }
        public string loading { get; set; }

        public string extension { get; set; }

    }
}
