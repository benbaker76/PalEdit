using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PalEdit
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            //args = new string[] { "C:\\Projects\\GitHub\\Crunchy\\Crunchy\\bin\\Debug\\net8.0-windows\\Graphics\\AnimatedSprites\\AnimatedSprites.ini" };
 
            int argCount = args == null ? 0 : args.Length;

            if (argCount > 0)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    AttachConsole(ATTACH_PARENT_PROCESS);
                }

                var version = Assembly.GetExecutingAssembly().GetName().Version;

                Console.WriteLine("PalEdit {0}", version.ToString(3));

                Colors.LoadPalettes();
                Colors.BatchProcessIniFile(args[0]);

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    FreeConsole();
                }

                return 1;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());

            return 1;
        }
    }
}