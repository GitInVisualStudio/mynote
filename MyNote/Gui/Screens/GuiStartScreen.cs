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
            Components.Add(new GuiTable<string>(new Vector(0, 0), "Hallo", "Hallo2", "Hallo3", "YEAHHH")
            {
                RWidth = 0.5f,
                RHeight = 1,
                Size = new Vector(-50, 0)                
            });
            Components.Add(new GuiTable<string>(new Vector(0, 0), "Hallo1", "Hallo2", "Hallo3", "YEAHHH")
            {
                RWidth = 0.5f,
                RHeight = 1,
                RX = 0.5f
            });
            Name = "Start";
        }
    }
}
