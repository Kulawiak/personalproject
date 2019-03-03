#region References

using System;
using System.IO;
using System.Net;
using System.Text;

#endregion

namespace LED.SACN.Helpers
{
    public class Reader : BinaryReader
    {
        public Reader(Stream input) : base(input)
        {

        }

        public Reader(Stream input, Encoding encoding) : base(input, encoding)
        {

        }

        public override short ReadInt16() => IPAddress.NetworkToHostOrder(BitConverter.ToInt16(base.ReadBytes(2), 0));

        public override ushort ReadUInt16() => (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(base.ReadBytes(2), 0));

        public override int ReadInt32() => IPAddress.NetworkToHostOrder(BitConverter.ToInt32(base.ReadBytes(4), 0));
    }
}
