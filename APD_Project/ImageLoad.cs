using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APD_Project
{
    internal class ImageLoad
    {
        public static Image LoadImage(string url)
        {
            HttpWebRequest httpWeb = (HttpWebRequest)WebRequest.Create(url);
            httpWeb.UserAgent = "Chrome/105.0.0.0";
            HttpWebResponse webResponse = (HttpWebResponse)httpWeb.GetResponse();
            Stream stream = webResponse.GetResponseStream();
            Bitmap bitmap = new Bitmap(stream);
            stream.Dispose();
            return bitmap;
        }
    }
}
