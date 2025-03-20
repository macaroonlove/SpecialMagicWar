using FrameWork;
using SpecialMagicWar.Save;
using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// �÷��̾� ����(����) ��ȯ�ϴ� �ý���
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

        /// <summary>
        /// �������� ��忡�� ����ϴ� ��
        /// </summary>
        internal void CreateBot(int id)
        {
            var agentSystem = BattleManager.Instance.GetSubSystem<AgentSystem>();
            var poolSystem = CoreManager.Instance.GetSubSystem<PoolSystem>();

            AddressableAssetManager.Instance.GetScriptableObject<AgentTemplate>($"Player{id}", (template) =>
            {
                // ���� �����ϱ�
                var obj = poolSystem.Spawn(template.prefab, transform.GetChild(id));

                obj.transform.SetPositionAndRotation(transform.GetChild(id).position, Quaternion.identity);

                if (obj.TryGetComponent(out AgentUnit unit))
                {
                    // ���� �ʱ�ȭ
                    unit.Initialize(template);

                    // ���� ���
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