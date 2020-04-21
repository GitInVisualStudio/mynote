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
        public GuiStartScreen(Vector size) : base(size)
        {
            Components.Add(new GuiTable<string>(new Vector(50, 50), "Hallo", "Hallo2", "Hallo3", "YEAHHH"));
            Components.Add(new GuiTable<string>(new Vector(size.X / 2 + 500, 50), "Hallo1", "Hallo2", "Hallo3", "YEAHHH"));
        }
    }
}
