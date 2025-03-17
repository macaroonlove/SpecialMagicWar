using UnityEngine;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// �������� ����ϴ� �ð��� �����ϴ� Ŭ����
    /// </summary>
    public class TimeSystem : MonoBehaviour, IBattleSystem
    {
        private float _startTime;

        internal float currentTime => Time.time - _startTime;

        public void Initialize()
        {
            _startTime = Time.time;
        }

        public void Deinitialize()
        {

        }
    }
}