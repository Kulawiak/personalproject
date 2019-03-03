#region References

using System;

using LED.SACN.Layers;

#endregion

namespace LED.SACN
{
    public class Packet
    {
        public const ushort Flags      = 0x7 << 12;
        public const ushort Begin_Mask = 0b1111_0000_0000_0000;
        public const ushort End_Mask   = 0b0000_1111_1111_1111;
        public const int Max_Size      = 638;

        /// <summary>
        /// Root Layer
        /// </summary>
        public Root _Root
        {
            get;
            set;
        }

        /// <summary>
        /// Source Name
        /// </summary>
        public string Source
        {
            get
            {
                return _Root._Framing.Source;
            }
            set
            {
                _Root._Framing.Source = value;
            }
        }

        /// <summary>
        /// GU Identifier
        /// </summary>
        public Guid GUID
        {
            get
            {
                return _Root.GUID;
            }

            set
            {
                _Root.GUID = value;
            }
        }

        /// <summary>
        /// Sequence Identifier
        /// </summary>
        public byte Sequence
        {
            get
            {
                return _Root._Framing.Sequence;
            }

            set
            {
                 _Root._Framing.Sequence = value;
            }
        }

        /// <summary>
        /// Data of packet
        /// </summary>
        public byte[] Data
        {
            get
            {
                return _Root._Framing._DMP.Data;
            }

            set
            {
                _Root._Framing._DMP.Data = value;
            }
        }

        public ushort Unviverse
        {
            get
            {
                return _Root._Framing.Universe;
            }

            set
            {
                _Root._Framing.Universe = value;
            }
        }

        public Packet(ushort _Universe, string _Source, Guid _GUID, byte _Sequence, byte[] _Data, byte _Priority = 100)
        {
            _Root = new Root(_GUID, _Source, _Universe, _Sequence, _Data, _Priority);
        }

        public Packet(Root __Root)
        {
            _Root = __Root;
        }

        public byte[] Array()
        {
            return _Root.Array();
        }
    }
}
