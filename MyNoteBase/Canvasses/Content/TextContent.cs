using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MyNoteBase.Canvasses.Content
{
    public class TextContent : CanvasContent
    {
        private List<TextChar> chars;

        public List<TextChar> Chars
        {
            get
            {
                return chars;
            }

            set
            {
                chars = value;
            }
        }

        public TextChar this[int i]
        {
            get
            {
                return chars[i]; 
            }
        }

        public TextContent(JObject json) : base(json)
        {
            this.Chars = json["chars"].ToObject<List<TextChar>>();
        }

        public TextContent() : base()
        {
            this.chars = new List<TextChar>();
        }

        public override Image Render()
        {
            throw new NotImplementedException(); // jamin todo
        }

        /// <summary>
        /// Inserts a character at a certain Position
        /// </summary>
        /// <param name="position">Default -1 means the character will be appended to the end</param>
        public void InsertChar(TextChar c, int position = -1)
        {
            if (position == -1)
                chars.Add(c);
            else
                chars.Insert(position, c);
        }

        public void RemoveChar(int position)
        {
            chars.RemoveAt(position);
        }
    }
}
