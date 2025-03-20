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
            int botCount = BattleManager.Instance.botCount;
            if (botCount > 0)
            {
                Vector3 botPos = pos;
                Vector3 botWayPoint = pos;
                botWayPoint.y = 4;

                for (int i = 1; i <= botCount; i++)
                {
                    botPos.x += 1.38f;
                    botWayPoint.x += 1.38f;

                    var unit = SpawnEnemyUnit(template, botPos);

                    unit.SetBotIndex(i);

                    var wayPoint = new List<Vector3>();
                    wayPoint.Add(botWayPoint);
                    unit.GetAbility<MoveWayPointAbility>().InitializeWayPoint(wayPoint);
                }
            }

            return SpawnEnemyUnit(template, pos);
        }

        internal EnemyUnit SpawnBountyUnit(EnemyTemplate template, Vector3 pos, int botIndex = 0)
        {
            if (botIndex == 0) return SpawnEnemyUnit(template, pos);
            else if (botIndex == 1) return SpawnEnemyUnit(template, pos + new Vector3(1.38f, 0, 0));
            else if (botIndex == 2) return SpawnEnemyUnit(template, pos + new Vector3(2.76f, 0, 0));
            else return SpawnEnemyUnit(template, pos + new Vector3(4.14f, 0, 0));
        }

        private EnemyUnit SpawnEnemyUnit(EnemyTemplate template, Vector3 pos)
        {
            if (_poolSystem == null) return null;

            // 유닛 생성하기
            var obj = _poolSystem.Spawn(template.prefab, transform);

            // 유닛 위치 정해주기 (위치가 타일과 같을 경우 타일에서 위치 정해주기도 가능)
            obj.transform.SetPositionAndRotation(pos, Quaternion.identity);

            if (obj.TryGetComponent(out EnemyUnit unit))
            {
                // 유닛 초기화
                unit.Initialize(template);

                // 유닛 등록
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