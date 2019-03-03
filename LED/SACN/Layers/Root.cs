#region References

using System;
using System.IO;

using LED.SACN.Helpers;

#endregion

namespace LED.SACN.Layers
{
    public class Root
    {
        static readonly short Pre_Length = 0x0010;
        static readonly short Post_Length = 0x0000;
        static readonly int Root_Vector = 0x00000004;
        static readonly byte[] Indentifier = new byte[] { 0x41, 0x53, 0x43, 0x2d, 0x45, 0x31, 0x2e, 0x31, 0x37, 0x00, 0x00, 0x00 };

        /// <summary>
        /// Packet's Framing Layer
        /// </summary>
        public Framing _Framing
        {
            get;
            set;
        }

        /// <summary>
        /// Length of Framing Layer as well as 38 accounting of other space
        /// </summary>
        public short Length
        {
            get
            {
                return (short)(38 + _Framing.Length);
            }
        }

        /// <summary>
        /// GU Identifier
        /// </summary>
        public Guid GUID
        {
            get; set;
        }

        public Root(Guid _GUID, string Soruce, ushort Universe, byte Sequence, byte[] Data, byte Priority)
        {
            GUID = _GUID;
            _Framing = new Framing(Soruce, Universe, Sequence, Data, Priority);
        }

        public Root()
        {

        }

        public byte[] Array()
        {
            using (MemoryStream Stream = new MemoryStream(Length))
            using (Writer Buffer = new Writer(Stream))
            {
                Buffer.Write(Pre_Length);
                Buffer.Write(Post_Length);
                Buffer.Write(Indentifier);
                Buffer.Write((ushort)(Packet.Flags | (ushort)(Length - 16)));
                Buffer.Write(Root_Vector);
                Buffer.Write(GUID.ToByteArray());

                Buffer.Write(_Framing.Array());

                return Stream.ToArray();
            }
        }
    }
}
