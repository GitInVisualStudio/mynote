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
        private readonly DrawContent drawContent;
        private List<T> list;
        private Bitmap table;
        public event EventHandler<T> ListClick;
        public int Length => list.Count;
        private int hoverIndex = -1;

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
            drawContent = (int index) =>
            {
                StateManager.DrawCenteredString(this[index].ToString(), Size.X / 2, 10);
                return 30;
            };
        }

        public GuiTable(Vector location, DrawContent drawContent, params T[] list) : base(location)
        {
            this.list = new List<T>();
            this.list.AddRange(list);
            this.drawContent = drawContent;
        }

        public override void Init()
        {
            base.Init();
            OnClick += GuiTable_OnClick;
            OnMove += GuiTable_OnMove;
            OnLeave += (object sender, Vector location) => hoverIndex = -1;
        }

        private void GuiTable_OnMove(object sender, Vector e)
        {
            for (int i = 0; i < Length; i++)
            {
                if (e.Y > 30 * i && e.Y < i * 30 + 30)
                    hoverIndex = i;
            }
        }

        /// <summary>
        /// Gibt die höhe des gerenderten zurück und wird automatisch auf passende höhe translated => Y-Koordinate immer 0 beim rendern!
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public delegate float DrawContent(int index);

        private void GuiTable_OnClick(object sender, Vector e)
        {
            for (int i = 0; i < Length; i++)
            {
                //TODO: call the right component
            }
        }

        private void CreateNewBitmap()
        {
            if (Size.X < 1 || Size.Y < 1) //lul
                return;
            table?.Dispose();
            table = new Bitmap((int)Size.X, (int)Size.Y);
            DrawTable();
        }

        private void DrawTable()
        {
            StateManager.Push();
            StateManager.SetGraphics(Graphics.FromImage(table));
            #region drawingTable
            StateManager.SetColor(Color.Black);
            StateManager.DrawString(Name + ":", 0, 0);
            float offsetY = StateManager.GetStringHeight(Name + ":");
            StateManager.SetFont(FontUtils.DEFAULT_FONT_BOLD);
            StateManager.Translate(0, offsetY);
            StateManager.SetColor(Color.Gray);
            StateManager.FillRoundRect(0, 0, (int)Size.X, (int)Size.Y - offsetY);
            StateManager.Push();
            StateManager.SetColor(Color.Black);
            for (int i = 0; i < Length; i++)
            {
                float height = drawContent.Invoke(i);
                if (hoverIndex == i)
                    StateManager.DrawRect(0, 0, Size.X, height);
                StateManager.Translate(0 , height);
                offsetY += height;
                if (offsetY > Size.Y) //Scrolling later lol
                    break;
            }
            #endregion
            StateManager.Pop();
            StateManager.Pop();
        }

        public void Add(T t)
        {
            list.Add(t);
            CreateNewBitmap();
        }

        public override void OnRender()
        {
            if (table == null)
                CreateNewBitmap();
            if(table.Width != Size.X || table.Height != Size.Y) //Nicht im event => StateManager asyn zugriff & setzen von den Graphics
                CreateNewBitmap();
            StateManager.DrawImage(table, Location.X , Location.Y);
        }
    }
}
