using MyNote.Utils.Math;
using MyNote.Utils.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Gui.Screen
{
    public class GuiHeader : GuiScreen
    {
        private Vector prevLocation = new Vector(0,0);
        private bool onMove = false;

        public GuiHeader(MyNote myNote) : base(myNote)
        {
            Components.Add(new GuiButton("X", (Vector location) => myNote.Close())
            {
                RX = 1,
                Location = new Vector(-30, 0),
                Size = new Vector(30, 30),
                HoverColor = Color.Red
            });

            OnClick += (object sender, Vector location) =>
            {
                prevLocation = location;
                onMove = true;
            };
            OnRelease += (object sender, Vector location) => onMove = false;
            OnMove += (object sender, Vector location) =>
            {
                if (!OnHover(location))
                    return;
                if(onMove)
                    myNote.Location = new Point(
                        myNote.Location.X - (int)(prevLocation.X - location.X),
                        myNote.Location.Y - (int)(prevLocation.Y - location.Y));
            };
            Name = "MyNote";
        }

        public override void OnRender()
        {
            StateManager.SetColor(Color.LightGray);
            StateManager.FillRect(Location, Size);
            StateManager.SetColor(Color.Black);
            StateManager.DrawCenteredString(Name, Size / 2);

            base.OnRender();
        }
    }
}
