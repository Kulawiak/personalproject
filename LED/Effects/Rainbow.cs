#region References 

using System.Threading;

using LED.Values;

#endregion

namespace LED.Effects
{
    public class Rainbow
    {
        public Rainbow()
        {
            new Thread(() =>
            {

                byte[][] Pattern = { Colors.Red, Colors.Orange, Colors.Yellow, Colors.Green, Colors.Light_Blue, Colors.Blue, Colors.Purple, Colors.Pink };

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
