using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class StatusInstance
    {
        public Coroutine corutine;
        public float duration;
        public float startTime;
        
        public bool useCountLimit = false;
        public int count = int.MaxValue;

        public StatusInstance(float duration, float time)
        {
            this.duration = duration;
            this.startTime = time;
        }

        public bool IsCompete => duration == 0 || Time.time - startTime > duration;

        /// <summary>
        /// �����Ű� �� ������ ��� true
        /// </summary>
        internal bool IsOld(float duration)
        {
            if (this.duration == 0) return true;
            if (duration == 0) return false;

            return (this.duration - Time.time - startTime) < duration;
        }
    }
}
