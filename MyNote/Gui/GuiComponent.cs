using MyNote.Utils.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Gui
{
    public abstract class GuiComponent
    {
        private Vector location;
        private Vector size;
        private string name;
        private bool selected;
        private Color backColor = Color.Gray;
        private Color fontColor = Color.Black;
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

        public string Name { get => name; set => name = value; }
        public bool Selected { get => selected; set => selected = value; }
        public Color BackColor { get => backColor; set => backColor = value; }
        public Color FontColor { get => fontColor; set => fontColor = value; }

        public void Component_OnResize(Vector size) => OnResize?.Invoke(this, size);
        public void Component_OnClick(Vector location) => OnClick?.Invoke(this, location);
        public void Component_OnRelease(Vector location) => OnRelease?.Invoke(this, location);
        public void Component_OnMove(Vector location) => OnMove?.Invoke(this, location);
        public void Component_OnKeyPress(char keyChar) => OnKeyPress?.Invoke(this, keyChar);
        public void Component_OnKeyRelease(char keyChar) => OnKeyRelease?.Invoke(this, keyChar);

        public bool OnHover(Vector location)
        {
            return location.X > this.location.X && location.X < this.location.X + size.X && location.Y > this.location.Y && location.Y < this.location.Y + size.Y;
        }

        public GuiComponent(float x, float y)
        {
            Location = new Vector(x, y);
            OnResize += SetLocationAndSize;
        }

        public GuiComponent(Vector location)
        {
            Location = location;
            Size = size;
            OnResize += SetLocationAndSize;
        }

        public abstract void Init();

        public abstract void OnRender();

        public abstract void SetLocationAndSize(object sender, Vector screenSize);
    }
}
