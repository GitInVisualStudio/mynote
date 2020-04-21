using MyNote.Utils.Math;
using MyNote.Utils.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Gui
{
    public class GuiButton : GuiComponent
    {
        private const float DEFAULT_WIDTH = 100; 
        private const float DEFAULT_HEIGHT = 20;

        //TODO: den kack mit den Size fixen lol idk wie ich es machen soll
        public GuiButton(string name) : base(0, 0)
        {
            Name = name;
        }

        public GuiButton(string name, EventHandler<Vector> Click) : base(0, 0)
        {
            Name = name;
            OnClick += Click;
        }


        public GuiButton(string name, float x, float y) : base(x, y)
        {
            Name = name;
        }

        public GuiButton(float x, float y, float width, float height) : base(x, y)
        {
        }

        public override void Init()
        {
            //throw new NotImplementedException(); TODO: IDK yet....
        }

        public override void OnRender()
        {
            //throw new NotImplementedException();
            //TODO: Render the shit
            StateManager.SetColor(BackColor);
            StateManager.FillRect(Location, Size);
            StateManager.SetColor(FontColor);
            StateManager.DrawRect(Location, Size);
            StateManager.DrawCenteredString(Name, Location + Size / 2);
        }

        public override void SetLocationAndSize(object sender, Vector screenSize)
        {
            
        }
    }
}
