using MyNote.Utils;
using MyNote.Utils.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Gui
{
    public class GuiScreen : Component
    {
        private List<Component> components;
        private Animation<float> animation = Animation<float>.GetDefaultAnimation();
        private bool opend;

        public List<Component> Components
        {
            get
            {
                return components;
            }

            set
            {
                components = value;
            }
        }

        public bool Opend
        {
            get
            {
                return opend;
            }

            set
            {
                opend = value;
            }
        }

        public GuiScreen(Vector size) : base(default(Vector), size)
        {
            OnResize += Screen_OnResize;
            OnClick += Screen_OnClick;
            OnMove += Screen_OnMove;
            OnRelease += Screen_OnRelease;
            OnKeyPress += Screen_OnKeyPress;
            OnKeyRelease += Screen_OnKeyRelease;
        }

        private void Screen_OnResize(object sender, Vector e)
        {
            components.ForEach(x => x.Component_OnResize(e));
        }

        private void Screen_OnRelease(object sender, Vector e)
        {
            components.ForEach(x => x.Component_OnRelease(e));
        }

        private void Screen_OnMove(object sender, Vector e)
        {
            components.ForEach(x => x.Component_OnMove(e));
        }

        private void Screen_OnKeyRelease(object sender, char e)
        {
            components.ForEach(x => x.Component_OnKeyRelease(e));
        }

        private void Screen_OnKeyPress(object sender, char e)
        {
            components.ForEach(x => x.Component_OnKeyPress(e));
        }

        private void Screen_OnClick(object sender, Vector e)
        {
            components.ForEach(x => x.Component_OnClick(e));
        }

        public void Open()
        {
            animation.StartAnimation();
        }

        public void Close()
        {
            animation.InvertAnimation();
        }

        public override void Init()
        {
            components.ForEach(x => x.Init());
        }

        public override void OnRender()
        {
            components.ForEach(x => x.OnRender());
        }
    }
}
