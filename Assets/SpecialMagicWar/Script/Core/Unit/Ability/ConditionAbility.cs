using UnityEngine;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// ���ǿ� �´� �ɷ� ��, ���� �켱������ ���� �ɷ¸� ����
    /// </summary>
    public abstract class ConditionAbility : Ability
    {
        [SerializeField] private int _priority;

        public int priority => _priority;

        /// <summary>
        /// ���డ�� ���θ� ��ȯ�ϴ� �߻� �޼���
        /// </summary>
        internal abstract bool IsExecute();

        /// <summary>
        /// �ɷ��� �ٽ� ����� �� ȣ��
        /// </summary>
        internal virtual void StartAbility()
        {

        }

        /// <summary>
        /// �ɷ��� ������ �� ȣ��
        /// </summary>
        internal virtual void StopAbility()
        {

        }
    }
}