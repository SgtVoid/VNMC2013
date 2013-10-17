using Microsoft.Xna.Framework.Media;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace VNMC2013.DropBox
{
    public class Client
    {
        private static RestClient restClient = new RestClient(ApiContentBaseUrl);
        private static RestRequest request;

        private const string ApiContentBaseUrl = "https://api-content.dropbox.com";
        private const string UserSecret = "M_JHdKiVkwAAAAAAAAAAAa0UD-U5CRPm4AGfdQYoAE4HJ-sYfZM9LH0yVTA3QoUY";

        public async static Task<string> UploadFile(string filename, Stream photo)
        {
            filename = Regex.Match(filename, @"[A-Za-z0-9_-]*\.*\w+$").Groups[0].Value;
            string url = string.Format("/1/files/sandbox/VNMC2013?access_token={0}&file={1}", UserSecret, filename);

            request = new RestRequest(url, Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddFile("file", ReadFully(photo), filename);

            await restClient.ExecuteTaskAsync(request);
            return filename;
        }

        public async static Task<Picture> GetFile(string filename)
        {
            string url = string.Format("/1/files/sandbox/VNMC2013/{0}?access_token={1}", filename, UserSecret);
            request = new RestRequest(url, Method.GET);

            IRestResponse response = await restClient.ExecuteTaskAsync(request);

            MediaLibrary library = new MediaLibrary();
            return library.SavePicture(filename, response.RawBytes);
        }

        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[input.Length];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
