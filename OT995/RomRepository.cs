using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OT995
{
    class RomRepository 
    {
        public Stream GetRomList(String url)
        {
                WebRequest request = WebRequest.Create(
                    "http://www.ot995toolbox.net84.net/Files/filelist.csv");

                WebResponse response = request.GetResponse();

                var file = response.GetResponseStream();

            return file;
        }

    }
}
