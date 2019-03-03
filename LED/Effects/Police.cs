#region References 

using System.Threading;

using LED.Values;

#endregion

namespace LED.Effects
{
    public class Police
    {
        public Police()
        {
            new Thread(() =>
            {
                byte[][] Pattern = { Colors.Blue, Colors.Red };

                Program.data = _Extensions.Get_Array(Pattern);

                Program.stop = false;
                while (!Program.stop)
                {
                    _Extensions.Scroll();
                }
            }).Start();
        }
    }
}
