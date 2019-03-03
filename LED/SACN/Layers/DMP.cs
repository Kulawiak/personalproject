#region References

using System.IO;

using LED.SACN.Helpers;

#endregion

namespace LED.SACN.Layers
{
    public class DMP
    {
        static readonly byte DMP_Vector = 2;
        static readonly byte Address_And_Data_Type = 0xa1;
        static readonly short First_Address = 0;
        static readonly short Address_Increment = 1;
        static readonly byte Zero = 0x00;

        /// <summary>
        /// Starting Code
        /// </summary>
        public byte Start_Code
        {
            get;
            set;
        }

        /// <summary>
        /// Content (Data)
        /// </summary>
        public byte[] Data
        {
            get;
            set;
        }

        /// <summary>
        /// Length of Data + 11
        /// </summary>
        public short Length
        {
            get
            {
                return (short)(11+ Data.Length);
            }
        }

        public DMP(byte[] _Data)
        {
            Data = _Data;
        }

        public byte[] Array()
        {
            using (MemoryStream Stream = new MemoryStream(Length))
            using (Writer Buffer = new Writer(Stream))
            {
                Buffer.Write((ushort)(Packet.Flags | Length));
                Buffer.Write(DMP_Vector);
                Buffer.Write(Address_And_Data_Type);
                Buffer.Write(First_Address);
                Buffer.Write(Address_Increment);
                Buffer.Write((short)(Data.Length + 1));
                Buffer.Write(Start_Code);
                Buffer.Write(Data);

                return Stream.ToArray();
            }
        }
    }
}
