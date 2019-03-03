#region References

using System;
using System.IO;
using System.Text;

using static System.Console;

using LED.Values;

#endregion

namespace LED.Helpers.Console
{
    internal class Modifier : TextWriter
    {
        #region TextWriter Declaration

        public readonly TextWriter Output = null;

        #endregion

        #region Startup

        public Modifier()
        {
            Output = Out;
        }

        #endregion

        #region Adjust Spacing for Centering Stats

        public static string Adjust_Spacing(string _String)
        {
            if (_String.Length < 38)
            {
                string Adjusted = _String;
                int Change = 38 - _String.Length;
                for (int i = 0; i < Change; i++)
                {
                    Adjusted = Adjusted.Insert(Adjusted.IndexOf("→") + 1, " ");
                }
                return Adjusted;
            }
            return _String;
        }

        #endregion

        #region Encode

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

        #endregion

        #region Write

        public override void Write(string _String)
        {
            Output.Write(_String);
        }

        #endregion

        #region Write + Custom Color

        public override void Write(string _String, object Color)
        {
            ForegroundColor = (ConsoleColor)Color;
            Output.Write(_String);
            ResetColor();
        }

        #endregion

        #region Write Empty Line

        public override void WriteLine()
        {
            Output.WriteLine();
        }

        #endregion

        #region Write Line

        public override void WriteLine(string _String)
        {
            ForegroundColor = ConsoleColor.DarkRed;
            Write($"{Constants.Product}    ");
            ResetColor();
            Output.WriteLine(_String);
        }

        #endregion

        #region Write Line With a Custom Object

        public override void WriteLine(string _String, object Obj)
        {
            ForegroundColor = ConsoleColor.DarkRed;
            Write($"{Constants.Product}    ");
            ResetColor();

            if (Obj is bool)
            {
                if ((bool)Obj)
                {
                    ForegroundColor = ConsoleColor.Cyan;
                    Output.WriteLine(_String);
                    ResetColor();
                }
                else
                {
                    Output.WriteLine(_String);
                }
            }
            else if (Obj is ConsoleColor)
            {
                ForegroundColor = (ConsoleColor)Obj;
                Output.WriteLine(_String);
                ResetColor();
            }
            else
            {
                Output.WriteLine(_String);
            }
        }

        #endregion

        #region Write Centered Line with Color

        public override void WriteLine(string _String, object Obj, object Obj2)
        {
            if (Obj is int)
            {
                if ((int)Obj == 0)
                {
                    if (Obj2 == null)
                    {
                        SetCursorPosition((WindowWidth - _String.Length) / 2, CursorTop);
                        Output.WriteLine(_String);
                        SetCursorPosition(CursorLeft, CursorTop);
                    }
                    else
                    {
                        if (Obj2 is ConsoleColor)
                        {
                            ForegroundColor = (ConsoleColor)Obj2;
                        }

                        SetCursorPosition((WindowWidth - _String.Length) / 2, CursorTop);
                        Output.WriteLine(_String);
                        SetCursorPosition(CursorLeft, CursorTop);
                        ResetColor();
                    }
                }
            }
            else
            {
                ForegroundColor = ConsoleColor.DarkRed;
                Write($"{Constants.Product}    ");
                ResetColor();

                if (Obj is bool)
                {
                    if ((bool)Obj)
                    {
                        ForegroundColor = ConsoleColor.Cyan;
                        Output.WriteLine(_String);
                        ResetColor();
                    }
                    else
                    {
                        Output.WriteLine(_String);
                    }
                }
                else if (Obj is ConsoleColor)
                {
                    ForegroundColor = (ConsoleColor)Obj;
                    Output.WriteLine(_String);
                    ResetColor();
                }
                else
                {
                    Output.WriteLine(_String);
                }
            }
        }

        #endregion
    }
}
