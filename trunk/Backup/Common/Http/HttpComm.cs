using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;

namespace Common.Http
{
    public static class HttpComm
    {
        public static StringBuilder GetPage(string adresa)
        {
            // used to build entire input
            StringBuilder sb = new StringBuilder();

            // used on each read operation
            byte[] buf = new byte[8192];

            // prepare the web page we will be asking for
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(adresa);
            request.UserAgent = Properties.Settings.Default.NazivServisa + ";" + Properties.Settings.Default.NazivServisaDuzi;
            request.Accept = "Accept-Charset: utf-8";

            Stream resStream;
            try
            {
                // execute the request
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // we will read data via the response stream
                resStream = response.GetResponseStream();
            }
            catch (Exception ex)
            {
                Common.EventLogger.WriteEventError("Greška pri čitanju URL-a.", ex);
                return null;
            }


            string tempString = null;
            int count = 0;

            do
            {
                // fill the buffer with data
                count = resStream.Read(buf, 0, buf.Length);

                // make sure we read some data
                if (count != 0)
                {
                    // translate from bytes to UTF8 text
                    tempString = Encoding.UTF8.GetString(buf, 0, count);

                    // continue building the string
                    sb.Append(tempString);
                }
            }
            while (count > 0); // any more data to read?

            // print out page source
            //Console.WriteLine(sb.ToString());
            return sb;
        }
    }
}
