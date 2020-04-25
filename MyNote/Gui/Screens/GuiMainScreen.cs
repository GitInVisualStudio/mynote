using MyNote.Utils.Math;
using MyNote.Utils.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyNote.Gui.Screens
{
    public class GuiMainScreen : GuiScreen
    {
        private const int MARGIN = 1;
        private List<OpenedScreen> openedScreens;
        private GuiScreen currentScreen;

        public GuiMainScreen(MyNote myNote) : base(myNote)
        {
        }

        public override void Init()
        {
            base.Init();
            openedScreens = new List<OpenedScreen>();
            openedScreens.Add(new OpenedScreen("+", (x) => {
                OpenScreen(new GuiStartScreen(MyNote));
            })
            {
                Location = new Vector(0, 0),
                Size = new Vector(100, 30)
            });
            Components.AddRange(openedScreens);

            OpenScreen(new GuiStartScreen(MyNote));

            OnResize += (object sender, Vector size) =>
            {
                currentScreen?.Component_OnResize(size);
                MyNote.Refresh();
            };

            OnMove += (object sender, Vector location) => currentScreen?.Component_OnMove(location);
        }

        public void OpenScreen(GuiScreen screen)
        {
            if(openedScreens.Find((x) => x.Screen == screen) == null)
            {
                OpenedScreen openedScreen = new OpenedScreen(screen)
                {
                    Size = new Vector(100, 30),
                    Location = openedScreens.Last().Location + new Vector(100 + MARGIN, 0)
                };
                openedScreens.Add(openedScreen);
                Components.Add(openedScreen);
                new Thread(() =>
                {
                    screen.Init();
                    currentScreen = screen;
                }).Start();
            }
        }

        public override void SetLocationAndSize(object sender, Vector screenSize)
        {
            base.SetLocationAndSize(sender, screenSize);
            currentScreen?.SetLocationAndSize(sender, screenSize);
        }

        public void CloseScreen(GuiScreen screen) => openedScreens.Remove(openedScreens.Find(x => x.Screen == screen));

        public override void OnRender()
        {
            StateManager.SetColor(Color.Gray);
            StateManager.FillRect(0, 0, Size.X, 20);
            base.OnRender();
            StateManager.SetColor(Color.LightGray);
            StateManager.FillRect(0, 30 - 10, Size.X, Size.Y);
            currentScreen?.OnRender();
        }

    }

    class OpenedScreen : GuiButton
    {
        private GuiScreen screen;

        public OpenedScreen(GuiScreen screen) : base(screen.Name)
        {
            this.Screen = screen;
            BackColor = Color.LightGray;
        }

        public OpenedScreen(string name, Action<Vector> click) : base(name, click)
        {
            screen = null;
            BackColor = Color.LightGray;
        }

        public GuiScreen Screen { get => screen; set => screen = value; }

        public override void OnRender()
        {
            int /*var1 = 100, var2 = 30,*/ var3 = 12/*, var4 = 5*/;
            StateManager.SetColor(BackColor);
            StateManager.FillRoundRect(Location, Size, var3);
            StateManager.SetColor(Color.Black);
            StateManager.DrawCenteredString(Name, Location + Size / 2);
        }
    }
}
