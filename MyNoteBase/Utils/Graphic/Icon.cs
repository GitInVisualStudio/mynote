using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyNoteBase.Utils.Graphic
{
    [Serializable]
    public class Icon
    {
        private string name;
        private Image img;

        public string Name { get => name; set => name = value; }
        public Image Img { get => img; set => img = value; }

        public Icon(string name, Image img)
        {
            this.name = name;
            this.img = img;
        }

        public Icon(string name, string imgPath)
        {
            this.name = name;
            this.img = Image.FromStream(new FileStream(imgPath, FileMode.Open));
        }
    }
}
