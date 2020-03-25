using MyNoteBase.Canvasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Utils.Render
{
    public class TestIManager : IManager
    {
        private Image img;

        public Image GetImage()
        {
            if (img == null)
                img = new Bitmap("D:\\Bilder\\lulz\\665.jpg");
            return img;
        }

        public void SetImage(Image img)
        {
            this.img = img;
        }
    }
}
