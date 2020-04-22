using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNote.Utils.Math;
using MyNote.Utils.Render;

namespace MyNote.Gui
{
    public class GuiTable<T> : GuiComponent
    {
        private DrawContent drawContent;
        private List<T> list;
        private Bitmap table;
        public event EventHandler<T> ListClick;
        public int Length => list.Count;

        public T this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                list[index] = value;
            }
        }

        public int IndexOf(T t) => list.IndexOf(t);

        public GuiTable(Vector location, params T[] list) : base(location)
        {
            this.list = new List<T>();
            this.list.AddRange(list);
            OnClick += GuiTable_OnClick;
            drawContent = (int index) => StateManager.DrawCenteredString(this[index].ToString(), Size.X / 2, index * 20 + 10);
        }

        public GuiTable(Vector location, DrawContent drawContent, params T[] list) : base(location)
        {
            this.list = new List<T>();
            this.list.AddRange(list);
            OnClick += GuiTable_OnClick;
            this.drawContent = drawContent;
        }

        public delegate void DrawContent(int index);

        private void GuiTable_OnClick(object sender, Vector e)
        {
            for (int i = 0; i < Length; i++)
            {
                //TODO: call the right component
            }
        }

        private void CreateNewBitmap()
        {
            if (Size.Length == 0)
                return;
            table = new Bitmap((int)Size.X, (int)Size.Y);
            DrawTable();
        }

        private void DrawTable()
        {
            StateManager.Push();
            StateManager.SetGraphics(Graphics.FromImage(table));
            #region drawingTable
            StateManager.SetColor(Color.Gray);
            StateManager.FillRoundRect(0, 0, (int)Size.X, (int)Size.Y);
            StateManager.SetColor(Color.Black);
            for (int i = 0; i < Length; i++)
            {
                drawContent?.Invoke(i);
            }
            #endregion
            StateManager.Pop();
        }

        public void Add(T t)
        {
            list.Add(t);
            CreateNewBitmap();
        }

        public override void Init()
        {
            CreateNewBitmap();
        }

        public override void OnRender()
        {
            if(table != null)
                StateManager.DrawImage(table, Location.X , Location.Y);
        }

        public override void SetLocationAndSize(object sender, Vector screenSize)
        {
            base.SetLocationAndSize(sender, screenSize);
            CreateNewBitmap();
        }
    }
}
