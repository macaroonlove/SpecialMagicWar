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

        private int _spawnIndex;
        private int _spawnBot1Index;
        private int _spawnBot2Index;
        private int _spawnBot3Index;

        public void Initialize()
        {
            _agentSystem = BattleManager.Instance.GetSubSystem<AgentSystem>();
            _poolSystem = CoreManager.Instance.GetSubSystem<PoolSystem>();

            _spawnIndex = 0;
            _spawnBot1Index = 0;
            _spawnBot2Index = 0;
            _spawnBot3Index = 0;
        }

        public void Deinitialize()
        {

        }

        internal bool CreateUnit(HolyAnimalTemplate template, int botIndex = 0)
        {
            int spawnIndex;
            if (botIndex == 0) spawnIndex = _spawnIndex;
            else if (botIndex == 1) spawnIndex = _spawnBot1Index;
            else if (botIndex == 2) spawnIndex = _spawnBot2Index;
            else spawnIndex = _spawnBot3Index;

            if (spawnIndex >= 7) return false;

            var obj = _poolSystem.Spawn(template.prefab, transform.GetChild(botIndex).GetChild(spawnIndex));
            obj.transform.SetPositionAndRotation(transform.GetChild(botIndex).GetChild(spawnIndex).position, Quaternion.identity);

            if (obj.TryGetComponent(out HolyAnimalUnit unit))
            {
                // 유닛 초기화
                unit.Initialize(template);

                // 유닛 등록
                _agentSystem.Regist(unit, botIndex);

                if (botIndex == 0) _spawnIndex++;
                else if (botIndex == 1) _spawnBot1Index++;
                else if (botIndex == 2) _spawnBot2Index++;
                else _spawnBot3Index++;
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