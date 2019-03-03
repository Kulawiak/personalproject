#region References

using System;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;

#endregion

namespace LED.SACN
{
    public class Sender
    {
        /// <summary>
        /// GUID for identification
        /// </summary>
        public Guid GUID
        {
            get;
            set;
        }

        /// <summary>
        /// UDP Socket for connection
        /// </summary>
        private UdpClient Socket
        {
            get;
            set;
        }

        /// <summary>
        /// Destination's IP Address
        /// </summary>
        public IPAddress Address
        {
            get;
            set;
        }

        /// <summary>
        /// Multicast support
        /// </summary>
        public bool Multicast
        {
            get
            {
                return Address == null;
            }
        }

        /// <summary>
        /// Destion's port
        /// </summary>
        public int Port
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the source from which the data is being sent
        /// </summary>
        public string Source
        {
            get;
            set;
        }

        /// <summary>
        /// Identifiers for each packet's sequence
        /// </summary>
        private Dictionary<ushort, byte> Sequence_Identifiers
        {
            get;
            set;
        }

        public Sender(Guid _GUID, string _Source)
        {
            Port   = 5568;
            GUID   = _GUID;
            Source = _Source;
            Socket = new UdpClient();
            Sequence_Identifiers = new Dictionary<ushort, byte>();
        }

        public async Task Send(ushort Universe, byte[] Data)
        {
            Sequence_Identifiers.TryGetValue(Universe, out byte Sequence_ID);
            Packet _Packet = new Packet(Universe, Source, GUID, Sequence_ID++, Data);
            Sequence_Identifiers[Universe] = Sequence_ID;

            byte[] Packet_Bytes = _Packet.Array();
            await Socket.SendAsync(Packet_Bytes, Packet_Bytes.Length, GetEndPoint(Universe, Port));
        }

        public async Task Send(string Host, ushort Universe, byte[] Data)
        {
            Sequence_Identifiers.TryGetValue(Universe, out byte Sequence_ID);
            Packet _Packet = new Packet(Universe, Source, GUID, Sequence_ID++, Data);
            Sequence_Identifiers[Universe] = Sequence_ID;

            byte[] Packet_Bytes = _Packet.Array();
            await Socket.SendAsync(Packet_Bytes, Packet_Bytes.Length, Host, Port);
        }

        private IPEndPoint GetEndPoint(ushort Universe, int Port)
        {
            if (Multicast)
            {
                return new IPEndPoint(Get_Multicast(Universe), Port);
            }
            else
            {
                return new IPEndPoint(Address, Port);
            }
        }

        private static IPAddress Get_Multicast(ushort Universe)
        {
            byte[] Universe_Bytes = BitConverter.GetBytes(Universe).Reverse().ToArray();
            return new IPAddress(new byte[] { 239, 255, Universe_Bytes[0], Universe_Bytes[1]});
        }

        public void Close()
        {
            Socket.Close();
        }
    }
}
