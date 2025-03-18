using FrameWork;
using SpecialMagicWar.Save;
using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    public class PlayerCreateSystem : MonoBehaviour
    {
        internal event UnityAction<AgentUnit> onCreatePlayer;

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
    }
}