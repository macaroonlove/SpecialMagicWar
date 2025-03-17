using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class EnemySpawnSystem : MonoBehaviour, IBattleSystem
    {
        private EnemySystem _enemySystem;
        private PoolSystem _poolSystem;

        public void Initialize()
        {
            _poolSystem = CoreManager.Instance.GetSubSystem<PoolSystem>();
            _enemySystem = BattleManager.Instance.GetSubSystem<EnemySystem>();
        }

        public void Deinitialize()
        {
            _poolSystem = null;
        }

        internal EnemyUnit SpawnUnit(EnemyTemplate template, Vector3 pos)
        {
            // ���� �����ϱ�
            var obj = _poolSystem.Spawn(template.prefab, transform);

            // ���� ��ġ �����ֱ� (��ġ�� Ÿ�ϰ� ���� ��� Ÿ�Ͽ��� ��ġ �����ֱ⵵ ����)
            obj.transform.SetPositionAndRotation(pos, Quaternion.identity);

            if (obj.TryGetComponent(out EnemyUnit unit))
            {
                // ���� �ʱ�ȭ
                unit.Initialize(template);

                // ���� ���
                _enemySystem.Regist(unit);

                return unit;
            }
            else
            {
                _poolSystem.DeSpawn(obj);
                return null;
            }
        }
    }
}