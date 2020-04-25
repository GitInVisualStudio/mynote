﻿using MyNote.Utils;
using MyNote.Utils.Math;
using MyNote.Utils.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Gui
{
    public class GuiButton : GuiComponent
    {
        private const float DEFAULT_WIDTH = 100; 
        private const float DEFAULT_HEIGHT = 20;
        private Color hoverColor;
        private Color currentColor;
        public Color HoverColor { get => hoverColor; set => hoverColor = value; }
        private Animation<float> animation = Animation<float>.GetDefaultAnimation();
        private Action customRender = null;

        public override Color BackColor { get => base.BackColor; set => currentColor = base.BackColor = value; }
        public Action CustomRender { get => customRender; set => customRender = value; }

        //TODO: den kack mit den Size fixen lol idk wie ich es machen soll
        public GuiButton(string name) : base(0, 0)
        {
            Name = name;
        }

        public GuiButton(string name, Action<Vector> click) : base(0, 0)
        {
            Name = name;
            OnClick += (object sender, Vector location) => click.Invoke(location); //Like wtf ?? OnClick += click;
        }


        public override void Init()
        {
            animation.StartAnimation();
            
            base.Init();
            OnEnter += (object sender, Vector location) =>
            {
                currentColor = HoverColor;
                animation.InvertAnimation();
            };

            OnLeave += (object sender, Vector location) =>
            {
                currentColor = BackColor;
                animation.InvertAnimation();
            };
        }

        public override void OnRender()
        {
            //throw new NotImplementedException();
            //TODO: Render the shit
            if (customRender == null)
            {
                StateManager.SetColor(currentColor);
                StateManager.FillRect(Location, Size);
                StateManager.SetColor(FontColor);
                StateManager.DrawCenteredString(Name, Location + Size / 2);
            }
            else
                customRender.Invoke();
        }
    }
}
