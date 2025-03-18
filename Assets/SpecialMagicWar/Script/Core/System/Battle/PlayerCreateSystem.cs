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
                // ���� �����ϱ�
                var obj = poolSystem.Spawn(template.prefab, transform.GetChild(0));

                // ���� ��ġ �����ֱ� (TODO: ������ ������ ���, �濡 ���� ���̵� ������ ��ġ)
                obj.transform.SetPositionAndRotation(transform.GetChild(0).position, Quaternion.identity);

                if (obj.TryGetComponent(out AgentUnit unit))
                {
                    // ���� �ʱ�ȭ
                    unit.Initialize(template);

                    // ���� ���
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