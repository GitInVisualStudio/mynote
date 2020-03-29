using MyNote.Utils;
using MyNote.Utils.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Gui
{
    public class GuiScreen : GuiComponent
    {
        private List<GuiComponent> components;
        private Animation<float> animation = Animation<float>.GetDefaultAnimation();
        private bool opend;

        public List<GuiComponent> Components
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
            components = new List<GuiComponent>();
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
            components.ForEach(x =>
            {
                if (x.OnHover(e))
                    x.Component_OnRelease(e);
            });
        }

        private void Screen_OnMove(object sender, Vector e)
        {
            components.ForEach(x =>
            {
                if (x.OnHover(e))
                    x.Component_OnMove(e);
            });
        }

        private void Screen_OnClick(object sender, Vector e)
        {
            components.ForEach(x =>
            {
                if (x.OnHover(e))
                    x.Component_OnClick(e);
            });
        }

        private void Screen_OnKeyRelease(object sender, char e)
        {
            components.ForEach(x =>
            {
                if (x.Selected)
                    x.Component_OnKeyRelease(e);
            });
        }

        private void Screen_OnKeyPress(object sender, char e)
        {
            components.ForEach(x=>
            {
                if (x.Selected)
                    x.Component_OnKeyPress(e);
            });
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
