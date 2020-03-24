using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Utils
{
    public class AnimationManager
    {
        public static List<Animation> animations = new List<Utils.Animation>();

        public static void Update()
        {
            for(int i = animations.Count - 1; i >= 0; i--)
            {
                animations[i].Update();
                if (!animations[i].Alive)
                    animations.RemoveAt(i);
            }
        }
    }
}
