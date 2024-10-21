using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.Helpers
{
    public static class UrlHelper
    {
        public static string GetFileNameFromUrl(string url)
        {
            Uri uri = new Uri(url);
            string fileName = uri.Segments.Last();
            return fileName;
        }
    }
}
