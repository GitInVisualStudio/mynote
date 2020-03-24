using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNoteBase.Classes;

namespace MyNoteBase.Canvasses
{
    public class Excercise : Canvas
    {
        public Excercise(DateTime dt, string name, Course course, IManager manager) : base(dt, name, course, manager)
        {
        }
    }
}
