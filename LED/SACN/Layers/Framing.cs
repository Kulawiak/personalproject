#region References

using System.IO;
using System.Linq;
using System.Text;

using LED.SACN.Helpers;

#endregion

namespace LED.SACN.Layers
{
    public class Framing
    {
        static readonly int Framing_Vector = 0x00000002;
        static readonly short Reserved = 0;
        static readonly int Source_Length = 64;

        /// <summary>
        /// DMP Layer
        /// </summary>
        public DMP _DMP
        {
            get;
            set;
        }

        /// <summary>
        /// Framing Length
        /// </summary>
        public short Length
        {
            get
            {
                return (short)(16 + Source_Length + _DMP.Length);
            }
        }

        /// <summary>
        /// Source's Name
        /// </summary>
        public string Source
        {
            get;
            set;
        }

        /// <summary>
        /// Universe Identifier
        /// </summary>
        public ushort Universe
        {
            get;
            set;
        }

        /// <summary>
        /// Sequence Identifier
        /// </summary>
        public byte Sequence
        {
            get;
            set;
        }

        /// <summary>
        ///  Layer Priority
        /// </summary>
        public byte Priority
        {
            get;
            set;
        }

        /// <summary>
        /// Framing Options
        /// </summary>
        public Options _Options
        {
            get;
            set;
        }

        public Framing(string _Source, ushort _Universe, byte _Sequence, byte[] Data, byte _Priority)
        {
            Source = _Source;
            Universe = _Universe;
            Sequence = _Sequence;
            _Options = new Options();
            _DMP = new DMP(Data);
            Priority = _Priority;
        }

        public Framing()
        {

        }

        public byte[] Array()
        {
            using (MemoryStream Stream = new MemoryStream(Length))
            using (Writer Buffer = new Writer(Stream))
            {
                Buffer.Write((short)(Packet.Flags | Length));
                Buffer.Write(Framing_Vector);
                Buffer.Write(Encoding.UTF8.GetBytes(Source));
                Buffer.Write(Enumerable.Repeat((byte)0, 64 - Source.Length).ToArray());
                Buffer.Write(Priority);
                Buffer.Write(Reserved);
                Buffer.Write(Sequence);
                Buffer.Write(_Options.ToByte());
                Buffer.Write(Universe);

                Buffer.Write(_DMP.Array());

                return Stream.ToArray();
            }
        }
    }
}
