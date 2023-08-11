using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PalEdit
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            int argCount = args == null ? 0 : args.Length;

            if (argCount > 0)
            {
                AttachConsole(ATTACH_PARENT_PROCESS);

                Console.WriteLine("PalEdit {0}", Globals.Version);

                Colors.LoadPalettes();
                Colors.BatchProcessIniFile(args[0]);

                return 1;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());

            return 1;
        }
    }
}