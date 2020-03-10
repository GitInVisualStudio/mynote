using MyNote.Utils.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Gui
{
    public abstract class Component
    {
        private Vector location;
        private Vector size;
        private string name;
        public event EventHandler<Vector> OnResize;
        public event EventHandler<Vector> OnClick;
        public event EventHandler<Vector> OnMove;
        public event EventHandler<Vector> OnRelease;
        public event EventHandler<char> OnKeyPress;
        public event EventHandler<char> OnKeyRelease;

        public Vector Location
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
            }
        }

        public Vector Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
            }
        }

        public void Component_OnResize(Vector location) => OnResize?.Invoke(this, location);
        public void Component_OnClick(Vector location) => OnClick?.Invoke(this, location);
        public void Component_OnRelease(Vector location) => OnRelease?.Invoke(this, location);
        public void Component_OnMove(Vector location) => OnMove?.Invoke(this, location);
        public void Component_OnKeyPress(char keyChar) => OnKeyPress?.Invoke(this, keyChar);
        public void Component_OnKeyRelease(char keyChar) => OnKeyRelease?.Invoke(this, keyChar);


        public Component(float x, float y, float width, float height)
        {
            Location = new Vector(x, y);
            Size = new Vector(width, height);
        }

        public Component(Vector location, Vector size)
        {
            Location = location;
            Size = size;
        }

        public abstract void Init();

        public abstract void OnRender();
    }
}
