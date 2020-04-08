using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    [JsonObject(MemberSerialization.Fields)]
    public class Icon
    {
        private string name;
        [JsonConverter(typeof(IO.ImageConverter))]
        private Image img;

        public string Name { get => name; set => name = value; }
        public Image Img { get => img; set => img = value; }

        public Icon(JObject json)
        {
            this.name = json["name"].ToObject<string>();

            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new IO.ImageConverter());
            this.img = json["img"].ToObject<Image>(serializer);
        }

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
