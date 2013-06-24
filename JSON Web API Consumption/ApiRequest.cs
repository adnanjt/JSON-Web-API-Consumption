using System;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
//using System;
//using System.Data;
//using System.Configuration;
//using System.Collections;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
using System.IO;

namespace JSON_Web_API_Consumption
{
	public class ApiRequest
	{
		readonly static WebClient webclient = new WebClient();

		public static string GetJson(Uri uri)
		{

			//return WebClient.DownloadString(uri);

            //using (WebClient client = new WebClient())
            //{
            //   NameValueCollection postData = new NameValueCollection() 
            //   { 
            //          //{ "image", file_get_contents("../blusa1.jpg") }  //order: {"parameter name", "parameter value"}
            //          { "image", Encoding.Default.GetString(ReadImage("../blusa1.jpg", new string[] { ".GIF", ".gif", ".jpg", ".bmp" })) } 
            //   };
            //   Console.WriteLine(Encoding.Default.GetString(ReadImage("../blusa1.jpg", new string[] { ".GIF", ".gif", ".jpg", ".bmp" })));
            //    // client.UploadValues returns page source as byte array (byte[])
            //    // so we need to transform that into string
            //   //Console.WriteLine(file_get_contents("../blusa1.jpg"));
            //   return Encoding.UTF8.GetString(client.UploadValues(uri, postData));
            //}
            string address = "http://localhost/pruebaColoresApi.php";
            string dumpPath = "../campe.jpg";

            using (var stream = File.Open(dumpPath, FileMode.Open))
            {
                var files = new[] 
                {
                    new UploadFile
                    {
                        Name = "file",
                        Filename = Path.GetFileName(dumpPath),
                        ContentType = "text/plain",
                        Stream = stream
                    }
                };

                            var values = new NameValueCollection
                {
                    { "client", "VIP" },
                    { "name", "John Doe" },
                };

                byte[] result = UploadFiles(address, files, values);
                return Encoding.UTF8.GetString(result);
            }
		}

        public class UploadFile
        {
            public UploadFile()
            {
                ContentType = "application/octet-stream";
            }
            public string Name { get; set; }
            public string Filename { get; set; }
            public string ContentType { get; set; }
            public Stream Stream { get; set; }
        }

        public static byte[] UploadFiles(string address, IEnumerable<UploadFile> files, NameValueCollection values)
        {
            var request = WebRequest.Create(address);
            request.Method = "POST";
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            boundary = "--" + boundary;

            using (var requestStream = request.GetRequestStream())
            {
                // Write the values
                foreach (string name in values.Keys)
                {
                    var buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(values[name] + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                // Write the files
                foreach (var file in files)
                {
                    var buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}", file.Name, file.Filename, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}{1}", file.ContentType, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    file.Stream.CopyTo(requestStream);
                    buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                var boundaryBuffer = Encoding.ASCII.GetBytes(boundary + "--");
                requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);
            }

            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var stream = new MemoryStream())
            {
                responseStream.CopyTo(stream);
                return stream.ToArray();
            }
        }

        protected static string file_get_contents(string fileName)
        {
            string sContents = string.Empty;
            if (fileName.ToLower().IndexOf("http:") > -1)
            { // URL 
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] response = wc.DownloadData(fileName);
                sContents = System.Text.Encoding.ASCII.GetString(response);
            }
            else
            {
                // Regular Filename 
                System.IO.StreamReader sr = new System.IO.StreamReader(fileName);
                sContents = sr.ReadToEnd();
                sr.Close();
            }
            return sContents;
        }

        private static byte[] ReadImage(string p_postedImageFileName, string[] p_fileType)
        {
            bool isValidFileType = false;
            try
            {
                FileInfo file = new FileInfo(p_postedImageFileName);

                foreach (string strExtensionType in p_fileType)
                {
                    if (strExtensionType == file.Extension)
                    {
                        isValidFileType = true;
                        break;
                    }
                }
                if (isValidFileType)
                {
                    FileStream fs = new FileStream(p_postedImageFileName, FileMode.Open, FileAccess.Read);

                    BinaryReader br = new BinaryReader(fs);

                    byte[] image = br.ReadBytes((int)fs.Length);

                    br.Close();

                    fs.Close();

                    return image;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
	}
}
