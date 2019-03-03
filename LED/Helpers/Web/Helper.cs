#region References

using System;
using System.IO;
using System.Web;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.IO.Compression;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using static System.Console;

using LED.Values;

#endregion

namespace LED.Helpers.Web
{
    class Helper
    {
        #region Get Machine's Private IP Address

        public static string IP()
        {
            Dictionary<int, string> Adapters = new Dictionary<int, string>();

            IPAddress[] IPArray = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress IP in IPArray)
            {
                if (IP.AddressFamily == AddressFamily.InterNetwork)
                {
                    Adapters.Add(Adapters.Count, IP.ToString());
                }
            }

            if (Adapters.Count == 0)
            {
                WriteLine("Could not find any network adapters... you need an adapter to run this program.");
                return null;
            }
            else if (Adapters.Count == 1)
            {
                Stats.Private_IP = Adapters[0];
                return Adapters[0];
            }
            else
            {
                WriteLine();
                WriteLine("More than one network adapters were found:");
                foreach (var Adapter in Adapters)
                {
                    WriteLine($"    {Adapter.Key} : {Adapter.Value}");
                }
                WriteLine("Enter the key of the address you want to use (hit the key twice):");
                try
                {
                    string Address = Adapters[Convert.ToInt32(ReadKey().KeyChar.ToString())];
                    Stats.Private_IP = Address;
                    WriteLine();
                    return Address;
                }
                catch (Exception X)
                {
                    WriteLine($"[ERROR] {X.Message}");
                    WriteLine($"    Using the default value [{Adapters[0]}]\n");
                    Stats.Private_IP = Adapters[0];
                    return Adapters[0];
                }
            }
        }

        #endregion

        #region Get Machine's Public IP Address

        public static string Public_IP()
        {
            string IP = Regex.Replace(new WebClient().DownloadString("http://icanhazip.com"), @"\s+", string.Empty);

            if (IP != null)
            {
                return IP;
            }
            return null;
        }

        #endregion

        #region Check if Machine Supports HTTP Listeners

        public static void Check()
        {
            if (!HttpListener.IsSupported)
            {
                WriteLine("This Machine Doesn't Support HTTP Listeners.");
                WriteLine("Press any key to exit ...");
                ReadKey();
                Environment.Exit(0);
            }
            return;
        }

        #endregion

        #region Get a Parameter from a Query String

        public static string Get_Parameter(string Request, string Q)
        {
            return HttpUtility.ParseQueryString(Request).Get(Q);
        }

        #endregion

        #region Respond to a Context

        public static void Respond(HttpListenerContext Ctx, string Html)
        {
            byte[] Buffer = Encoding.UTF8.GetBytes(Html);

            if (Ctx.Request.Headers["Accept-Encoding"] == "gzip")
            {
                using (MemoryStream _Stream = new MemoryStream())
                {
                    using (GZipStream Zip = new GZipStream(_Stream, CompressionMode.Compress, true))
                    {
                        Zip.Write(Buffer, 0, Buffer.Length);
                    }

                    Buffer = _Stream.ToArray();
                }

                Ctx.Response.AddHeader("Content-Encoding", "gzip");
                Ctx.Response.ContentLength64 = Buffer.Length;
            }

            if (Html == HTML.Pages.Inject(Files.Not_Found))
                Ctx.Response.StatusCode = (int)HttpStatusCode.NotFound;
            else
                Ctx.Response.StatusCode = (int)HttpStatusCode.OK;

            Ctx.Response.OutputStream.Write(Buffer, 0, Buffer.Length);
            Ctx.Response.OutputStream.Close();
        }

        #endregion

        #region Redirect

        public static bool Redirect(HttpListenerContext Ctx)
        {
            string Url = Ctx.Request.Url.ToString();

            if (Url.Contains("://www."))
            {
                Ctx.Response.StatusCode = (int)HttpStatusCode.Moved;
                Ctx.Response.AddHeader("Location", Url.Replace("://www.", "://"));
                Ctx.Response.Close();
                return true;
            }
            return false;
        }

        #endregion

        #region Check User Uniqueness

        public static void Check_Uniqueness(IPAddress IP)
        {
            if (!Stats.Visitors.ContainsValue(IP.ToString()))
            {
                Stats.Visitors.Add(Stats.Visitors.Count, IP.ToString());
            }
        }

        #endregion
    }
}
