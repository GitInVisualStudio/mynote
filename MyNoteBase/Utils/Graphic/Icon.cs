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
        private int id;

        public string Name { get => name; set => name = value; }
        public Image Img { get => img; set => img = value; }
        public int Id { get => id; set => id = value; }

        public Icon(JObject json)
        {
            this.name = json["name"].ToObject<string>();
            this.id = json["id"].ToObject<int>();

            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new IO.ImageConverter());
            this.img = json["img"].ToObject<Image>(serializer);
        }

        /// <summary>
        /// LEGACY, icons should only be recieved through api or taken from disc
        /// </summary>
        public Icon(string name, Image img, int id)
        {
            this.name = name;
            this.img = img;
            this.id = id;
        }

        public Icon(string name, string imgPath)
        {
            this.name = name;
            this.img = Image.FromStream(new FileStream(imgPath, FileMode.Open));
        }
    }
}
