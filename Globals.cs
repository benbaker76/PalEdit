using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PalEdit
{
    public class Globals
    {
        public static string Version = "v3.0";

        public class Folders
        {
            public static string Gradients = Path.Combine(Application.StartupPath, "Gradients");
            public static string Palettes = Path.Combine(Application.StartupPath, "Palettes");
        }

        public class FileNames
        {
            public static string Ini = Path.Combine(Application.StartupPath, "PalEdit.ini");
        }
    }
}
