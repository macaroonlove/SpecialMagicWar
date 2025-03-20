using FrameWork;
using SpecialMagicWar.Save;
using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// 플레이어 유닛(샤먼) 소환하는 시스템
    /// </summary>
    public class PlayerCreateSystem : MonoBehaviour
    {
        internal event UnityAction<AgentUnit> onCreatePlayer;
        internal event UnityAction<AgentUnit, int> onCreateBot;

        internal void CreatePlayer()
        {
            var agentSystem = BattleManager.Instance.GetSubSystem<AgentSystem>();
            var poolSystem = CoreManager.Instance.GetSubSystem<PoolSystem>();

            var id = SaveManager.Instance.profileData.playerSkin;
            AddressableAssetManager.Instance.GetScriptableObject<AgentTemplate>($"Player{id}", (template) =>
            {
                // 유닛 생성하기
                var obj = poolSystem.Spawn(template.prefab, transform.GetChild(0));

                // 유닛 위치 정해주기 (TODO: 포톤을 연결할 경우, 방에 들어온 아이디 순서로 배치)
                obj.transform.SetPositionAndRotation(transform.GetChild(0).position, Quaternion.identity);

                if (obj.TryGetComponent(out AgentUnit unit))
                {
                    // 유닛 초기화
                    unit.Initialize(template);

                    // 유닛 등록
                    agentSystem.Regist(unit);
                    onCreatePlayer?.Invoke(unit);
                }
                else
                {
                    poolSystem.DeSpawn(obj);
                }
            });
        }

        /// <summary>
        /// 오프라인 모드에서 사용하는 봇
        /// </summary>
        internal void CreateBot(int id)
        {
            var agentSystem = BattleManager.Instance.GetSubSystem<AgentSystem>();
            var poolSystem = CoreManager.Instance.GetSubSystem<PoolSystem>();

            AddressableAssetManager.Instance.GetScriptableObject<AgentTemplate>($"Player{id}", (template) =>
            {
                // 유닛 생성하기
                var obj = poolSystem.Spawn(template.prefab, transform.GetChild(id));

                obj.transform.SetPositionAndRotation(transform.GetChild(id).position, Quaternion.identity);

                if (obj.TryGetComponent(out AgentUnit unit))
                {
                    // 유닛 초기화
                    unit.Initialize(template);

                    // 유닛 등록
                    agentSystem.Regist(unit);
                    onCreateBot?.Invoke(unit, id);
                }
                else
                {
                    poolSystem.DeSpawn(obj);
                }
            });
        }
    }
}