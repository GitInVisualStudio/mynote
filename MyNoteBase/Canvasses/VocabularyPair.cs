using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteBase.Canvasses
{
    [Serializable]
    public class VocabularyPair
    {
        private Image one;
        private Image two;

        public Image One { get => one; set => one = value; }
        public Image Two { get => two; set => two = value; }
        public Image this[int i]
        {
            get
            {
                if (i == 0)
                    return one;
                else if (i == 1)
                    return two;
                else
                    throw new IndexOutOfRangeException();
            }
        }

        public VocabularyPair(Image one, Image two)
        {
            this.one = one;
            this.two = two;
        }
    }
}
