using MyNoteBase.Classes;
using MyNoteBase.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyNoteBase.Canvasses
{
    [JsonObject(MemberSerialization.Fields)]
    public abstract class Canvas
    {
        [JsonIgnore]
        private IManager manager;
        private DateTime dt;
        private string name;
        [JsonIgnore]
        private Course course;
        private int onlineID;
        private string localID;
        private int courseOnlineID;
        private string courseLocalID;
        [JsonConverter(typeof(Utils.IO.ImageConverter))]
        private Image pixels;
        private Type type;

        public IManager Manager
        {
            get
            {
                return manager;
            }

            set
            {
                manager = value;
            }
        }

        public Course Course { get => course; set => course = value; }

        public DateTime Dt
        {
            get
            {
                return dt;
            }

            set
            {
                dt = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public Image Pixels { get => manager.GetImage(); set => manager.SetImage(value); }
        public int CourseOnlineID { get => courseOnlineID; set => courseOnlineID = value; }
        public string CourseLocalID { get => courseLocalID; set => courseLocalID = value; }
        public string LocalID { get => localID;  }
        public int OnlineID { get => onlineID; set => onlineID = value; }

        public Canvas(JObject json, IManager manager)
        {
            this.manager = manager;
            this.dt = json["dt"].ToObject<DateTime>();
            this.name = json["name"].ToObject<string>();
            this.onlineID = json["onlineID"].ToObject<int>();
            this.localID = json["localID"].ToObject<string>();
            this.courseOnlineID = json["courseOnlineID"].ToObject<int>();
            this.courseLocalID = json["courseLocalID"].ToObject<string>();
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new Utils.IO.ImageConverter());
            this.manager.SetImage(json["pixels"].ToObject<Image>(serializer));
            this.type = GetType();
        }

        public Canvas(DateTime dt, string name, Course course, IManager manager)
        {
            this.dt = dt;
            this.name = name;
            this.course = course;
            this.manager = manager;
            this.localID = Globals.GetLocalID("c_" + name, dt);
            this.onlineID = 0;
            this.courseLocalID = course.LocalID;
            this.courseOnlineID = course.OnlineID;
            this.type = GetType();
        }

        public void PrepareForSerialization()
        {
            pixels = Pixels;
        }

        public void InitAfterDeserialization(IManager manager)
        {
            this.manager = manager;
            manager.SetImage(pixels);
        }
    }
}
