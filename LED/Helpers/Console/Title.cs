#region References

using System;

using LED.Values;
using static LED.Values.Constants;

#endregion

namespace LED.Helpers.Console
{
    class Title
    {
        #region Refresh Title

        public static void Refresh()
        {
            System.Console.Title = $"{Product} {Name} | {Build} | © {DateTime.Now.Year} DominikFX | Total Requests: {Stats.Requests}";
        }

        #endregion

        #region Increment Request Count

        public static void Increment_Count()
        {
            Stats.Requests++;
            Refresh();
        }

        #endregion
    }
}
