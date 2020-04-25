using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNote.Utils.Math;

namespace MyNote.Gui.Screens
{
    public class GuiStartScreen : GuiScreen
    {
        public GuiStartScreen(MyNote myNote) : base(myNote)
        {
            Components.Add(new GuiTable<string>(new Vector(0, 0), "Mathe", "Deutsch", "IT", "Englisch")
            {
                RWidth = 1 / 2f,
                RHeight = 1f,
                Size = new Vector(-50, -200),
                Location = new Vector(50, 100),
                Name = "Neue Notiz"
            });
            Components.Add(new GuiTable<string>(new Vector(0, 0), "Mathe - Algebra", "Deutsch - Heinz", "IT - Projekt")
            {
                RWidth = 1 / 2f,
                RHeight = 1f,
                RX = 1 / 2f,
                Size = new Vector(-100, -200),
                Location = new Vector(50, 100),
                Name = "Zuletzt geöffnet"
            });
            Name = "Start";
        }

        public override void SetLocationAndSize(object sender, Vector screenSize)
        {
            base.SetLocationAndSize(sender, screenSize);
        }
    }
}
