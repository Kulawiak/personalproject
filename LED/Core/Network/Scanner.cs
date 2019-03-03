#region References

using System;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Net.NetworkInformation;

using LED.Values;

#endregion

namespace LED.Core.Network
{
    class Scanner
    {
        static Dictionary<string, string> _Search = new Dictionary<string, string>();

        private static void PingCompleted(object sender, PingCompletedEventArgs e)
        {
            // Get IP
            string IP = (string)e.UserState;
            if (e.Reply != null && e.Reply.Status == IPStatus.Success)
            {
                // Get Hostname, Mac, and Detemine if the device is a controller.
                string Name = Get_Name(IP);
                string MAC = Get_MAC(IP);
                bool Espressif = IsEsp(MAC);

                if (Constants.Debug) Console.WriteLine($"{IP}, {MAC}, {Name}, {Espressif}");

                if (Name == "Controller" || Espressif)
                {
                    _Search.TryGetValue(IP, out string test);
                    if (string.IsNullOrEmpty(test))
                    {
                        // Add device to search results
                        _Search.Add(IP, MAC);
                    }
                }
            }
        }

        public static List<string> Search()
        {
            if (Constants.Debug) Console.WriteLine("Searching for devices ...");
            
            // Start scanning the devices on the network to find the controller
            string[] IP = Get_Gateway().Split('.');
            List<string> Results = new List<string>();

            // Scanning algorithim uses only the last part of the IP
            for (int i = 2; i <= 255; i++)
            {
                Ping($"{IP[0]}.{IP[1]}.{IP[2]}.{i}", 4, 2500);
            }

            foreach (var Device in _Search)
            {
                Results.Add(Device.Key);
            }
            
            if (Constants.Debug) Console.WriteLine($"Search complete, ({Results.Count}) device(s) found");

            return Results;
        }

        public static void Ping(string IP, int attempts, int timeout)
        {
            for (int i = 0; i < attempts; i++)
            {
                new Thread(delegate ()
                {
                    try
                    {
                        Ping _Ping = new Ping();
                        _Ping.PingCompleted += new PingCompletedEventHandler(PingCompleted);
                        _Ping.SendAsync(IP, timeout, IP);
                    }
                    catch { }
                }).Start();
            }
        }

        static string Get_Gateway()
        {
            // Gets the IP of the current network interface connection
            string IP = null;

            foreach (NetworkInterface Interface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (Interface.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (GatewayIPAddressInformation Info in Interface.GetIPProperties().GatewayAddresses)
                    {
                        IP = Info.Address.ToString();
                    }
                }
            }

            return IP;
        }

        private static string Get_Name(string IP)
        {
            // Gets the hostname of the device, using an IP
            try
            {
                IPHostEntry Entry = Dns.GetHostEntry(IP);
                if (Entry != null) return Entry.HostName;
            }
            catch (SocketException) { }

            return null;
        }

        private static string Get_MAC(string IP)
        {
            // Gets the MAC address of the device from the IP
            Process _Process = Process.Start( new ProcessStartInfo() { FileName = "arp", Arguments = $"-a {IP}", UseShellExecute = false, RedirectStandardOutput = true, CreateNoWindow = true });
            string[] Sub = _Process.StandardOutput.ReadToEnd().Split('-');

            return Sub.Length >= 8
                ? $"{Sub[3].Substring(Math.Max(0, Sub[3].Length - 2))}-{Sub[4]}-{Sub[5]}-{Sub[6]}-{Sub[7]}-{Sub[8].Substring(0, 2)}".ToUpper()
                : "*THIS*PC*";
        }

        private static bool IsEsp(string MAC)
        {
            // Determines if device is from Espressif, Inc.
            if (MAC == "*THIS*PC*") return false;
            string[] Sub = MAC.Split('-');
            // Check accross a database of known OUIs
            return Constants.Espressif_OUIs.Contains($"{Sub[0]}-{Sub[1]}-{Sub[2]}");
        }
    }
}
