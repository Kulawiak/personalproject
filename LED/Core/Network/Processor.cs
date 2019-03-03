#region References

using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.IO.Compression;
using System.Collections.Generic;

using LED.Values;
using LED.Effects;
using LED.Helpers.Web;
using LED.Helpers.HTML;
using LED.Helpers.Console;

#endregion

namespace LED.Core.Network
{
    class Processor
    {
        public static void Handle(HttpListenerContext Ctx)
        {
            try
            {
                //Get the Context IP (IP of User)
                IPAddress IP = Ctx.Request.RemoteEndPoint.Address;
                //Get full url
                Uri Location = Ctx.Request.Url;
                //Get URL without any Query Requests 
                string Request = Uri.UnescapeDataString(Location.LocalPath.TrimStart('/'));
                //Get URL Queries
                string Query = Uri.UnescapeDataString(Location.Query);
                //Get Post Request with all Queries
                string Post = new StreamReader(Ctx.Request.InputStream).ReadToEnd();

                //For Debug Purposes
                if (Constants.Debug)
                {
                    Console.WriteLine($"Connection IP: {IP}", true);
                    Console.WriteLine($"Local Request: {Request}", true);
                    Console.WriteLine($"Query Request: {Query}", true);
                    Console.WriteLine($"Post  Request: {Post}\n", true);
                }

                //Increment the Requests Count
                Title.Increment_Count();
                //Check that the the Visistor IP is Unique and Increment the Stats Value if so
                Helper.Check_Uniqueness(IP);

                //Avoid 404 because of a slash
                if (Request.EndsWith("/"))
                {
                    Ctx.Response.StatusCode = (int)HttpStatusCode.Moved;
                    Ctx.Response.AddHeader("Location", Location.ToString()
                        .Remove(Location.GetLeftPart(UriPartial.Path).LastIndexOf("/"), 1));
                    Ctx.Response.Close();
                    return;
                }

                //Premanent 301 Redirects for WWW
                if (Helper.Redirect(Ctx))
                {
                    return;
                }

                //Switch the Request
                switch (Request)
                {
                    //Empty -> User is requesting the Home or Play Page
                    case "":
                        {
                            switch (Query)
                            {
                                //Just Home
                                case "":
                                    Respond(Ctx, Pages.Home());
                                    break;

                                //Other
                                default:
                                    Respond(Ctx, Pages.Not_Found());
                                    break;
                            }
                            break;
                        }

                    case "set":
                        {
                            if (Post != "")
                            {
                                string effect = Helper.Get_Parameter(Post, "effect");

                                _Extensions.Stop_All();

                                switch (effect)
                                {
                                    case "on":
                                        {
                                            Program.data = Enumerable.Repeat((byte)255, Constants.CHANNELS).ToArray();
                                            break;
                                        }

                                    case "off":
                                        {
                                            Program.data = new byte[Constants.CHANNELS];
                                            break;
                                        }

                                    case "red":
                                        {
                                            Program.data = _Extensions.Get_Array(new byte[] { 255, 0, 0 });
                                            break;
                                        }

                                    case "green":
                                        {
                                            Program.data = _Extensions.Get_Array(new byte[] { 0, 255, 0 });
                                            break;
                                        }

                                    case "blue":
                                        {
                                            Program.data = _Extensions.Get_Array(new byte[] { 0, 0, 255 });
                                            break;
                                        }

                                    case "audio":
                                        {
                                            Constants.Scroll = Convert.ToInt32(Helper.Get_Parameter(Post, "type"));
                                            Program.Effects.Audio = new Visualization();
                                            break;
                                        }

                                    case "fade":
                                        {
                                            Program.Effects.Fade = new Fade();
                                            break;
                                        }

                                    case "chase":
                                        {
                                            Program.Effects.Chase = new Chase();
                                            break;
                                        }

                                    case "july":
                                        {
                                            Program.Effects.July_4 = new July_4();
                                            break;
                                        }

                                    case "police":
                                        {
                                            Program.Effects.Police = new Police();
                                            break;
                                        }

                                    case "rainbow":
                                        {
                                            Program.Effects.Rainbow = new Rainbow();
                                            break;
                                        }

                                    case "street":
                                        {
                                            Program.Effects.Streetlight = new Streetlight();
                                            break;
                                        }

                                    case "xmas":
                                        {
                                            Program.Effects.Xmas = new Xmas();
                                            break;
                                        }

                                    default:
                                        {
                                            break;
                                        }
                                }
                            }

                            Respond(Ctx, Pages.Home());
                            break;
                        }

                    case "custom":
                        {
                            if (Post != "")
                            {
                                _Extensions.Stop_All();
                                string _Color = Helper.Get_Parameter(Post, "color");
                                if (_Color.Length != 7) Respond(Ctx, Pages.Home());
                                Program.data = _Extensions.Get_Array(_Color.Substring(1));
                            }
                            Respond(Ctx, Pages.Home());
                            break;
                        }

                    case "console":
                        {
                            if (Post != "")
                            {
                                string State = Helper.Get_Parameter(Post, "state");

                                switch (State)
                                {
                                    case "show":
                                        {
                                            Visible.Show();
                                            break;
                                        }

                                    case "hide":
                                        {
                                            Visible.Hide();
                                            break;
                                        }

                                    case "quit":
                                        {
                                            Respond(Ctx, "LightFX has successfully been quit.");
                                            Environment.Exit(0);
                                            break;
                                        }

                                    default:
                                        {
                                            break;
                                        }
                                }
                                
                            }
                            Respond(Ctx, Pages.Home());
                            break;
                        }

                    case "settings":
                        {
                            if (Post != "")
                            {
                                if (Post.Contains("scan"))
                                {

                                    List<string> Results = Scanner.Search();

                                    StringBuilder Builder = new StringBuilder();
                                    Builder.Append("\n<form action=\"/settings\" method=\"post\">\n");
                                    Builder.Append("    <select name=\"device\">\n");
                                    Builder.Append($"        <option value=\"_\">{Results.Count} Controller(s) Found</option>\n");
                                    foreach (string Controller in Results)
                                    {
                                        Builder.Append($"        <option value=\"{Controller}\">{Controller}</option>\n");
                                    }
                                    Builder.Append("    </select>\n");
                                    Builder.Append("    <button value=\"submit\">Select</button>\n");
                                    Builder.Append("</form>\n");

                                    Respond(Ctx, Pages.Home().Replace("---", Builder.ToString()));
                                }
                                else if (Post.Contains("device"))
                                {
                                    string _IP = Helper.Get_Parameter(Post, "device");
                                    if (_IP != "_")  Constants.IP = _IP;
                                    Respond(Ctx, Pages.Home());
                                }
                                else if (Post.Contains("ip_address"))
                                {
                                    string _IP = Helper.Get_Parameter(Post, "ip_address");

                                    if (IPAddress.TryParse(_IP, out IPAddress temp))
                                    {
                                        Constants.IP = _IP;
                                    }
                                    Respond(Ctx, Pages.Home());
                                }
                                else if (Post.Contains("led"))
                                {
                                    try
                                    {
                                        Constants.LED_COUNT = Convert.ToInt32(Helper.Get_Parameter(Post, "led"));
                                        Constants.CHANNELS = Constants.LED_COUNT * 3;
                                        Constants.FPS = Convert.ToInt32(Helper.Get_Parameter(Post, "fps"));
                                    }
                                    catch
                                    {
                                        // User didn't type in a digit 
                                    }
                                    Respond(Ctx, Pages.Home());
                                }
                                else
                                {
                                    Respond(Ctx, Pages.Home());
                                }
                            }
                            break;
                        }

                    //None of the Pages are Recognized, Send a 404 Page
                    default:
                        {
                            Respond(Ctx, Pages.Not_Found());
                            break;
                        }
                }
            }
            catch (Exception EX)
            {
                //Avoid Console Spam if it's just a Dropped Client Error
                if (!(EX is ProtocolViolationException || EX is HttpListenerException))
                {
                    Stats.Exceptions++;
                    Console.WriteLine($"An Unexpected Error Occured When Proccessing The Context: \n {EX}");
                }
            }
        }

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

            if (Html == Pages.Not_Found())
                Ctx.Response.StatusCode = (int)HttpStatusCode.NotFound;
            else
                Ctx.Response.StatusCode = (int)HttpStatusCode.OK;

            Ctx.Response.OutputStream.Write(Buffer, 0, Buffer.Length);
            Ctx.Response.OutputStream.Close();

            //For Debug Purposes
            if (Constants.Debug)
            {
                string Response = Html;

                if (Response.Length > 20)
                {
                    Response = Response.Substring(0, 20);
                }

                Console.WriteLine($"Response Sent: {Response}", true);
            }
        }

        #endregion
    }
}
