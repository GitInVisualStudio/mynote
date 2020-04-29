using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteBase.Canvasses.Content
{
    [JsonObject(MemberSerialization.Fields)]
    public struct TextChar
    {
        private string value;
        private Font font;
        private Color color;
        private bool special;

        public string Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }

        public Font Font
        {
            get
            {
                return font;
            }

            set
            {
                font = value;
            }
        }

        public Color Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }

        public static TextChar NewLineChar
        {
            get
            {
                return new TextChar("\n", null, Color.Black, true);
            }
        }

        public bool Special
        {
            get
            {
                return special;
            }

            set
            {
                special = value;
            }
        }

        public TextChar(string value, Font font, Color color, bool special = false)
        {
            this.value = value;
            this.font = font;
            this.color = color;
            this.special = special;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TextChar))
                return false;
            TextChar tc = (TextChar)obj;
            return tc.Value == value && tc.Font == font && tc.Color == color && tc.Special && special;
        }

        public static bool operator ==(TextChar a, TextChar b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(TextChar a, TextChar b)
        {
            return !a.Equals(b);
        }
    }
}
