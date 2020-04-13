using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteBase.Canvasses
{
    [JsonObject(MemberSerialization.Fields)]
    public class VocabularyPair
    {
        [JsonConverter(typeof(Utils.IO.ImageConverter))]
        private Image one;
        [JsonConverter(typeof(Utils.IO.ImageConverter))]
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

        public VocabularyPair(JObject json)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new Utils.IO.ImageConverter());
            one = json["one"].ToObject<Image>(serializer);
            two = json["two"].ToObject<Image>(serializer);
        }

        public VocabularyPair(Image one, Image two)
        {
            this.one = one;
            this.two = two;
        }
    }
}
