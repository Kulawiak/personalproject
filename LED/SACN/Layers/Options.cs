namespace LED.SACN.Layers
{
    public class Options
    {
        /// <summary>
        /// Preview Data?
        /// </summary>
        public bool Preview
        {
            get;
            set;
        }

        /// <summary>
        /// Stream Terminated?
        /// </summary>
        public bool Terminated
        {
            get;
            set;
        }

        /// <summary>
        /// Force Synchronization?
        /// </summary>
        public bool Force_Sync
        {
            get;
            set;
        }

        private static readonly byte Force_Synchronization = 0b0000_1000;
        private static readonly byte Stream_Terminated = 0b0000_0100;
        private static readonly byte Preview_Data = 0b0000_0010;

        public Options()
        {

        }

        public byte ToByte()
        {
            byte Value = 0;

            if(Preview)
            {
                return (byte)(Value | Preview_Data);
            }

            if (Terminated)
            {
                return (byte)(Value | Stream_Terminated);
            }

            if (Force_Sync)
            {
                return (byte)(Value | Force_Synchronization);
            }

            return Value;
        }
    }
}
