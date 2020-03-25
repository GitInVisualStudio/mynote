using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNoteBase.Classes;
using MyNoteBase.Utils.Exceptions;

namespace MyNoteBase.Canvasses
{
    [Serializable]
    public class VocabularyListing : Canvas
    {
        private List<VocabularyPair> pairs;

        public List<VocabularyPair> Pairs { get => pairs; set => pairs = value; }

        public VocabularyListing(DateTime dt, string name, Course course, IManager manager, List<VocabularyPair> pairs) : base(dt, name, course, manager)
        {
            this.pairs = pairs;
        }

        public static VocabularyListing FromImageLists(DateTime dt, string name, Course course, IManager manager, List<Image> one, List<Image> two)
        {
            if (one.Count != two.Count)
                throw new ImageListsNotEqualSizeException();

            List<VocabularyPair> pairs = new List<VocabularyPair>();
            for (int i = 0; i < one.Count; i++)
                pairs.Add(new VocabularyPair(one[i], two[i]));

            return new VocabularyListing(dt, name, course, manager, pairs);
        }
    }
}
