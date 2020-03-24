using MyNote.Utils.IO;
using MyNoteBase.Canvasses;
using MyNoteBase.Utils.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyNote
{
    public partial class MyNote : Form
    {
        private IOManager ioManager;
        private List<Canvas> openCanvasses;

        public MyNote()
        {
            InitializeComponent();
            this.ioManager = new IOManager(new SaveLoader());
        }

        public void OpenCanvas(string path)
        {
            openCanvasses.Add(ioManager.LoadCanvas(path));
        }

        public void SaveCanvasses()
        {
            foreach (Canvas c in openCanvasses)
                ioManager.SaveCanvas(c);
        }
    }
}
