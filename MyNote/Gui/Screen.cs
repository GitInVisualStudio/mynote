using MyNote.Utils.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Gui
{
    public class Screen : Component
    {
        private List<Component> components;

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
        
        public Screen(Vector size) : base(default(Vector), size)
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
            throw new NotImplementedException();
        }

        private void Screen_OnMove(object sender, Vector e)
        {
            throw new NotImplementedException();
        }

        private void Screen_OnKeyRelease(object sender, char e)
        {
            throw new NotImplementedException();
        }

        private void Screen_OnKeyPress(object sender, char e)
        {
            throw new NotImplementedException();
        }

        private void Screen_OnClick(object sender, Vector e)
        {
            throw new NotImplementedException();
        }

        public override void OnRender()
        {
            components.ForEach(x => x.OnRender());
        }
    }
}
