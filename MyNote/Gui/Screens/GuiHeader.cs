using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Gui.Screen
{
    public class GuiHeader : GuiScreen
    {
        private const float margin = 100;
        public GuiHeader() : base(new Utils.Math.Vector(1280, 720))
        {
            Components.Add(new GuiButton("Datei", margin * 0, 0));
            Components.Add(new GuiButton("Bearbeiten", margin * 1, 0));
            Components.Add(new GuiButton("Ansicht", margin * 2, 0));
            Components.Add(new GuiButton("Projekt", margin * 3, 0));
        }
    }
}
