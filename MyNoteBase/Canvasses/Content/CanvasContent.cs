using MyNote.Utils.Math;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteBase.Canvasses.Content
{
    [JsonObject(MemberSerialization.Fields)]
    public abstract class CanvasContent
    {
        private Vector position;
        private Type t;

        public Vector Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public CanvasContent(JObject json)
        {
            this.position = json["position"].ToObject<Vector>();
            this.t = GetType();
        }

        public CanvasContent()
        {
            this.t = GetType();
        }

        public abstract Image Render();
    }
}
