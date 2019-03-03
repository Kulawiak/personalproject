#region References

using System;
using System.Threading;

using LED.SACN;
using static LED.Values.Constants;

#endregion

namespace LED.Core.Network
{
    internal class SACN_Sender
    {
        public SACN_Sender()
        {
            new Thread(() =>
            {
                Sender _Sender = new Sender(Guid.NewGuid(), $"{Product} {Build}");
                Console.WriteLine("sACN DMX Class → OK");

                // Clears all the leds (to Multicast and saved IP)
                _Sender.Send(IP, 1, new byte[1344]).Wait();
                _Sender.Send(1, Program.data).Wait();

                while (IP != string.Empty)
                {
                    // Send the global data array
                    if (Program.data != null)
                    {
                        if (Multicast)
                            _Sender.Send(1, Program.data).Wait();
                        else
                            _Sender.Send(IP, 1, Program.data).Wait();
                    }
                    Thread.Sleep(1000 / FPS);
                }
            }).Start();
        }
    }
}
