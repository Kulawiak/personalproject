#region References

using System.IO;
using System.Net;

#endregion

namespace LED.SACN.Helpers
{
    internal class Writer : BinaryWriter
    {
        public Writer(Stream output) : base(output)
        {

        }

        public override void Write(short Value) => base.Write(IPAddress.HostToNetworkOrder(Value));

        public override void Write(ushort Value) => base.Write(IPAddress.HostToNetworkOrder((short)Value));

        public override void Write(int Value) => base.Write(IPAddress.HostToNetworkOrder(Value));
    }
}
