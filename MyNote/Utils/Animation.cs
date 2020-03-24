using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Utils
{
    public abstract class Animation
    {
        protected bool alive;

        public bool Alive
        {
            get
            {
                return alive;
            }

            set
            {
                alive = value;
            }
        }

        public virtual void StartAnimation() => alive = true;
        

        public virtual void StopAnimation() => alive = false;
        

        public virtual void ResetAnimation() => alive = true;

        public abstract void Update();
    }

    public class Animation<T> : Animation where T : struct
    {
        private T start;
        private T end;
        private T current;
        private Func<T, T, T> animate;

        public T End
        {
            get
            {
                return end;
            }

            set
            {
                end = value;
            }
        }

        public T Current
        {
            get
            {
                return current;
            }

            set
            {
                current = value;
            }
        }

        public T Start
        {
            get
            {
                return start;
            }

            set
            {
                start = value;
            }
        }
        public override void ResetAnimation() => current = start;

        public void InvertAnimation()
        {
            T start = Start;
            Start = end;
            end = start;
            alive = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="animate">current, end</param>
        public Animation(T start, T end, Func<T, T, T> animate)
        {
            Start = start;
            End = end;
            this.animate = animate;
        }

        public static Animation<float> GetDefaultAnimation()
        {
            return new Animation<float>(0, 1.0f, (float current, float end) =>
            {
                return (end - current) * 2;
            });
        }

        public override void Update() => Current = animate(current, end);
    }
}
