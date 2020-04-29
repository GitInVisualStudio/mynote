using MyNote.Utils.IO;
using MyNoteBase.Utils.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyNote
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IOManager manager = new IOManager(new SaveLoader());
            try
            {   
                manager.LoadGlobals();
            }
            catch
            {

            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MyNote());
            manager.SaveGlobals();
        }
    }
}
