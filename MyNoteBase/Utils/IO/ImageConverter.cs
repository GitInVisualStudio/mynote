using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteBase.Utils.IO
{
    // stolen from https://stackoverflow.com/a/44370397
    public class ImageConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var base64 = (string)reader.Value;
            // convert base64 to byte array, put that into memory stream and feed to image
            return Image.FromStream(new MemoryStream(Convert.FromBase64String(base64)));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var image = (Image)value;
            byte[] imageBytes;
            // save to memory stream in original format
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Bmp);
                imageBytes = ms.ToArray();
            }
            // write byte array, will be converted to base64 by JSON.NET
            writer.WriteValue(imageBytes);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Image);
        }
    }
}
