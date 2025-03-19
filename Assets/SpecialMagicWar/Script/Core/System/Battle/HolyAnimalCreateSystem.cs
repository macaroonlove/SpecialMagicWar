using UnityEngine;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// 신수 유닛을 소환하는 시스템
    /// </summary>
    public class HolyAnimalCreateSystem : MonoBehaviour, IBattleSystem
    {
        private AgentSystem _agentSystem;
        private PoolSystem _poolSystem;

        public void Initialize()
        {
            _agentSystem = BattleManager.Instance.GetSubSystem<AgentSystem>();
            _poolSystem = CoreManager.Instance.GetSubSystem<PoolSystem>();
        }

        public void Deinitialize()
        {

        }

        internal bool CreateUnit(HolyAnimalTemplate template)
        {
            int spawnIndex = _agentSystem.holyAnimalCount;
            if (spawnIndex >= 7) return false;

            var obj = _poolSystem.Spawn(template.prefab, transform.GetChild(spawnIndex));
            obj.transform.SetPositionAndRotation(transform.GetChild(spawnIndex).position, Quaternion.identity);

            if (obj.TryGetComponent(out HolyAnimalUnit unit))
            {
                // 유닛 초기화
                unit.Initialize(template);

                // 유닛 등록
                _agentSystem.Regist(unit);
            }
            else
            {
                _poolSystem.DeSpawn(obj);
                return false;
            }

            return true;
        }
    }
}