using GameFrameWork.Base;
using GameFrameWork.Pool;

namespace Game.GamePlay
{
    public class ItemTime :IReference
    {
        private float time;
        private float duration;
        public float Duration { get { return duration; }  set { duration = value; } }

        public static ItemTime Create(float time, float duration)
        {
            var item =ClassPool<ItemTime>.Spawn();
            item.time = time;
            item.duration = duration;
            return item;
        }

        public ItemTime()
        {
            
        }

        public ItemTime(float time, float duration)
        {
            this.time = time;
            this.duration = duration;
        }

        public void Update()
        {
            time += GameFrameWork.FrameWork.Instance.TimeComponent.GetScaledTime();
        }

        public int GetCycleCount()
        {
            var cycle = time / duration;
            var outCycle = (int)cycle;
            time -= outCycle * duration;
            return outCycle;
        }

        public void Release()
        {
            time = 0f;
            duration = 0f;
        }
    }
}