#region References

using System;
using System.Text;
using System.Threading;
using System.Diagnostics;

using static System.Console;

using LED.Values;
using LED.Helpers.Web;
using LED.Core.Network;

#endregion

namespace LED.Helpers.Console
{
    internal class Parser
    {
        public Parser()
        {
            InputEncoding = Encoding.UTF8;

            new Thread(() =>
            {
                while (Stats.Error == false)
                {
                    ConsoleKeyInfo _Command = ReadKey(false);

                    if (Stats.Error == false)
                    {
                        switch (_Command.Key)
                        {
                            case ConsoleKey.H:
                                {
                                    WriteLine("------==== Help + Menu ====------", 0, ConsoleColor.DarkRed);
                                    WriteLine("  Key:   Operation             ", 0, ConsoleColor.DarkCyan);
                                    WriteLine("  S  :   Server Statistics.    ", 0, null);
                                    WriteLine("  D  :   Enable Debug.         ", 0, null);
                                    //WriteLine("  G  :   Starts the Graphical User Interface.", 0, null);
                                    WriteLine("  C  :   Clear the Console.    ", 0, null);
                                    WriteLine("  Q  :   Quit.                 ", 0, null);
                                    WriteLine("  O  :   Open light controls.  ", 0, null);
                                    WriteLine("  1  :   Show console window.  ", 0, null);
                                    WriteLine("  0  :   Hide console window.  ", 0, null);
                                    WriteLine("  R  :   Restart.              ", 0, null);
                                    WriteLine("-----====  End of Menu  ====-----", 0, ConsoleColor.DarkRed);

                                    break;
                                }

                            case ConsoleKey.D1:
                                {
                                    Visible.Show();
                                    break;
                                }

                            case ConsoleKey.D0:
                                {
                                    Visible.Hide();
                                    break;
                                }

                            case ConsoleKey.Q:
                                {
                                    Environment.Exit(0);
                                    break;
                                }

                            case ConsoleKey.S:
                                {
                                    WriteLine("-----==== Server Stats ====-----", 0, ConsoleColor.DarkRed);
                                    WriteLine(Modifier.Adjust_Spacing($"Total Requests  → {Stats.Requests}"), 0, null);
                                    WriteLine(Modifier.Adjust_Spacing($"Unique Visitors → {Stats.Visitors.Count}"), 0, null);
                                    WriteLine(Modifier.Adjust_Spacing($"Exceptions      → {Stats.Exceptions}"), 0, null);
                                    WriteLine(Modifier.Adjust_Spacing($"Private IP      → {Stats.Private_IP}"), 0, null);
                                    WriteLine(Modifier.Adjust_Spacing($"Public IP       → {Stats.Public_IP}"), 0, null);
                                    WriteLine("-----==== End of Stats ====-----", 0, ConsoleColor.DarkRed);
                                    break;
                                }

                            case ConsoleKey.D:
                                {
                                    Constants.Debug = !Constants.Debug;
                                    WriteLine(Constants.Debug == true ? "-----==== Enabled Debug ====-----" : "-----==== Disabled Debug ====-----", 0, ConsoleColor.DarkRed);
                                    break;
                                }

                            case ConsoleKey.C:
                                {
                                    Clear();
                                    //Program.Log.Clear_Log();
                                    break;
                                }

                            case ConsoleKey.O:
                                {
                                    WriteLine($"Opening the URL: '{Http.URL}'");
                                    URL.Open(Http.URL);
                                    break;
                                }

                            case ConsoleKey.R:
                                {
                                    WriteLine("Restarting the Server...");
                                    Thread.Sleep(200);
                                    Process.Start($"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString()}.exe");
                                    Environment.Exit(0);
                                    break;
                                }

                            default:
                                {
                                    WriteLine("Unknown command, try again. Press 'H' for Help.");
                                    break;
                                }
                        }
                    }
                }
            }).Start();

            WriteLine("Parser Class → OK");
        }
    }
}
