#region References

using System;
using static System.Console;

using LED.Values;

#endregion

namespace LED.Helpers.HTML
{
    class Pages
    {
        #region Declarations

        const string Error = "The server encountered an internal error or misconfiguration and was unable to complete your request. (500)";

        #endregion

        #region Home Page

        public static string Home()
        {
            try
            {
                return Inject(Files.Home);
            }
            catch (Exception EX)
            {
                WriteLine($"An unexpected exception occured when reading an HTML file: \n{EX}");
                Stats.Exceptions++;
                return Error;
            }
        }

        #endregion

        #region 404 Page (Not Found)

        public static string Not_Found()
        {
            try
            {
                return Inject(Files.Not_Found);
            }
            catch (Exception EX)
            {
                WriteLine($"An unexpected exception occured when reading an HTML file: \n{EX}");
                Stats.Exceptions++;
                return Error;
            }
        }

        #endregion

        #region Inject Dpendencies to HTML

        public static string Inject(string HTML)
        {
            return HTML
                .Replace("%YYYY%", DateTime.Now.Year.ToString())
                .Replace("%fps%", Constants.FPS.ToString())
                .Replace("%led%", Constants.LED_COUNT.ToString())
                .Replace("%ip%", Constants.IP)
                .Replace("%BUILD_INFO%", $"{Constants.Product} {Constants.Name} | {Constants.Build} | &copy; {DateTime.Now.Year} DominikFX");
        }

        #endregion
    }
}
