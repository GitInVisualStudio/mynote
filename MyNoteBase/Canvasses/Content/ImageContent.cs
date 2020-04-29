using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace MyNoteBase.Canvasses.Content
{
    public class ImageContent : CanvasContent
    {
        [JsonConverter(typeof(Utils.IO.ImageConverter))]
        private Image img;
        public Image Img
        {
            get
            {
                return img;
            }

            set
            {
                img = value;
            }
        }

        public ImageContent(JObject json) : base(json)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new Utils.IO.ImageConverter());
            this.img = json["img"].ToObject<Image>(serializer);
        }

        public override Image Render()
        {
            throw new NotImplementedException();
        }
    }
}
