using FrameWork.Editor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// ������ �����ϴ� �̵� �����Ƽ
    /// </summary>
    public class MoveChaseAbility : MoveAbility
    {
        [SerializeField, ReadOnly] private float _chaseRange;
        [SerializeField, ReadOnly] private float _chaseFailRange;
        [SerializeField, ReadOnly] private float _stoppingDistance;
        [SerializeField, ReadOnly] private Transform _targetPosition;

        internal override void Initialize(Unit unit)
        {
            base.Initialize(unit);

            if (unit is AgentUnit agentUnit)
            {
                _chaseRange = agentUnit.template.ChaseRange;
                _chaseFailRange = agentUnit.template.ChaseFailRange * agentUnit.template.ChaseFailRange;
                _stoppingDistance = agentUnit.template.AttackRange * agentUnit.template.AttackRange;
            }
            else if (unit is EnemyUnit enemyUnit)
            {
                _chaseRange = enemyUnit.template.ChaseRange;
                _chaseFailRange = enemyUnit.template.ChaseFailRange * enemyUnit.template.ChaseFailRange;
                _stoppingDistance = enemyUnit.template.AttackRange * enemyUnit.template.AttackRange;
            }
        }

        internal override void UpdateAbility()
        {
            if (finalIsMoveAble == false) return;

            // ��ǥ Ÿ���� ���� ���
            if (_targetPosition == null)
            {
                if (unit is AgentUnit)
                {
                    var target = BattleManager.Instance.GetSubSystem<EnemySystem>().GetNearestEnemy(unit.transform.position, _chaseRange);

                    if (target != null)
                    {
                        _targetPosition = target.transform;
                    }
                }
                else if (unit is EnemyUnit)
                {
                    var target = BattleManager.Instance.GetSubSystem<AgentSystem>().GetNearestAgent(unit.transform.position, _chaseRange);

                    if (target != null)
                    {
                        _targetPosition = target.transform;
                    }
                }
            }

            #region �̵��ϱ�
            if (_targetPosition != null)
            {
                // ��ֹ��� ���� ��, ���� �̵�
                float distance = (_targetPosition.position - transform.position).sqrMagnitude;

                if (distance > _stoppingDistance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _targetPosition.position, finalMoveSpeed * Time.deltaTime);

                    MoveAnimation();
                }
                else
                {
                    StopMoveAnimation();
                }

                if (distance > _chaseFailRange)
                {
                    _targetPosition = null;
                }

                //// 3D ȯ�濡�� ��ֹ��� ���� �̵�
                //if (_navMeshAgent != null)
                //{
                //    _navMeshAgent.speed = finalMoveSpeed;
                //    _navMeshAgent.SetDestination(_targetPosition.position);
                //}

                //// 2D ȯ�濡�� ��ֹ��� ���� �̵�
                //var astar = BattleManager.Instance.GetSubSystem<AStarSystem>();
                //// TODO: ��ΰ� �ٲ�� ���(��ֹ� ���� ��)���� Path�� ��Ž���ϵ��� ����
                //var path = astar.SearchPath(transform.position, _targetPosition.position, AStarSystem.NodeTag.Ground);
                //astar.Move(transform, path, finalMoveSpeed);
            }
            #endregion

            #region ȸ���ϱ�
            if (_targetPosition != null)
            {
                Vector3 direction = (_targetPosition.position - transform.position).normalized;

                // 2D ȸ��
                FlipUnit(direction);

                // 3D ȸ��
                //RotateUnit(direction);
            }
            #endregion
        }
    }
}