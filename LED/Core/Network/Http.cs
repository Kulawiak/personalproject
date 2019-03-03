#region References

using System;
using System.Net;
using System.Threading;
using System.Collections.Generic;

using LED.Values;
using LED.Helpers.Web;

#endregion

namespace LED.Core.Network
{
    internal class Http
    {
        //Private Declarations The Listner itslef, the Port and the Root URL
        private static HttpListener Listener = null;
        private static readonly int Port = 80;

        //Using a Helper Method to get the Machine IP
        public static readonly string URL = Constants.Debug == true ? $"http://127.0.0.1:{Port}/" : $"http://{Helper.IP()}:{Port}/";

        public Http()
        {
            //Check if the Machine is compatible with HttpListeners
            Helper.Check();

            //Create a new Thread for the Listener
            new Thread(() =>
            {
                //Initialize the Stats Dictionary
                Stats.Visitors = new Dictionary<int, string>();

                //Declare a new Listner with an Anonymous Authentication Scheme
                Listener = new HttpListener()
                {
                    AuthenticationSchemes = AuthenticationSchemes.Anonymous
                };

                //Add Root URL
                Listener.Prefixes.Add(URL);

                //Attempt to Start the Listener, otherwise handle the Error thrown
                try
                {
                    Listener.Start();

                    //Startup Message
                    Console.WriteLine("HTTP Class → OK\n");
                    Console.WriteLine($"Listening for Connections on '{URL}'.\n");
                }
                catch (Exception EX)
                {
                    if (EX is HttpListenerException)
                    {
                        Console.Write("ERROR\n\n", ConsoleColor.Red);
                        Console.WriteLine($"Failed to Start the Server, Please check if the port '{Port}' is not in use.", ConsoleColor.Yellow);
                    }
                    else
                    {
                        Stats.Exceptions++;
                        Console.Write("ERROR\n\n", ConsoleColor.Red);
                        Console.WriteLine($"An unexpected error occured when starting the Server: \n{EX.Message}");
                    }

                    Stats.Error = true;
                    Console.WriteLine("Press any key to exit ...");
                    Console.ReadKey();
                    return;
                }

                //Add a Public IP
                Stats.Public_IP = Helper.Public_IP();

                //Create a Loop, which allows to handle multiple clients at the same time.
                while (Listener.IsListening)
                {
                    ThreadPool.QueueUserWorkItem((O) =>
                    {
                        //Handle the Connection Context
                        Processor.Handle((HttpListenerContext)O);
                    }, Listener.GetContext());
                }
            }).Start();
        }

        public static void Stop()
        {
            Listener.Stop();
        }
    }
}