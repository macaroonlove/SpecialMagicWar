using System.Collections.Generic;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// ���� ����Ʈ�� �����Ͽ� �̵��ϴ� �̵� �����Ƽ
    /// </summary>
    public class MoveWayPointAbility : MoveAbility
    {
        private Vector3 _targetPosition;
        private int _currentStep = 0;

        private List<Vector3> _wayPoint;

        internal void InitializeWayPoint(List<Vector3> wayPoint)
        {
            if (wayPoint.Count > 0)
            {
                _wayPoint = wayPoint;

                // �ʱ� ��ǥ ��ġ ����
                _targetPosition = wayPoint[_currentStep];
            }
        }

        internal override void UpdateAbility()
        {
            if (finalIsMoveAble == false) return;
            if (_wayPoint.Count == 0) return;

            float distance = Vector2.Distance(transform.position, _targetPosition);

            // ��ǥ ��ġ�� �����ϸ�
            if (distance < 0.01f)
            {
                StopMoveAnimation();
                return;
            }

            #region �̵��ϱ�
            // ��ֹ��� ���� ��, ���� �̵�
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, finalMoveSpeed * Time.deltaTime);
            #endregion

            #region ȸ���ϱ�
            Vector3 direction = (_targetPosition - (Vector3)transform.position).normalized;

            // 2D ȸ��
            FlipUnit(direction);
            #endregion

            MoveAnimation();
        }
    }
}