using UnityEngine;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// �ż� ������ ��ȯ�ϴ� �ý���
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
                // ���� �ʱ�ȭ
                unit.Initialize(template);

                // ���� ���
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