using DG.Tweening;
using FrameWork.Editor;
using UnityEngine;
using UnityEngine.AI;

namespace SpecialMagicWar.Core
{
    public class MoveAbility : ConditionAbility
    {
        [SerializeField, ReadOnly] private float _baseMoveSpeed;

        private BuffAbility _buffAbility;
        private AbnormalStatusAbility _abnormalStatusAbility;
        private UnitAnimationAbility _unitAnimationAbility;

        #region 3D�� ��� (2D�� ��� ����)
        protected NavMeshAgent _navMeshAgent;
        #endregion

        #region ��� ����
        protected float finalMoveSpeed
        {
            get
            {
                float result = _baseMoveSpeed;

                #region ����������
                float increase = 1;

                foreach (var effect in _buffAbility.MoveIncreaseDataEffects)
                {
                    increase += effect.value;
                }

                foreach (var effect in _abnormalStatusAbility.MoveIncreaseDataEffects)
                {
                    increase += effect.value;
                }

                result *= increase;
                #endregion

                #region ��¡��϶�
                foreach (var effect in _buffAbility.MoveMultiplierDataEffects)
                {
                    result *= effect.value;
                }
                #endregion

                return result;
            }
        }

        protected bool finalIsMoveAble
        {
            get
            {
                // �̵� �Ұ� �����̻� �ɷȴٸ�
                if (_abnormalStatusAbility.UnableToMoveEffects.Count > 0) return false;

                return true;
            }
        }
        #endregion

        internal override void Initialize(Unit unit)
        {
            base.Initialize(unit);

            TryGetComponent(out _navMeshAgent);

            _buffAbility = unit.GetAbility<BuffAbility>();
            _abnormalStatusAbility = unit.GetAbility<AbnormalStatusAbility>();
            _unitAnimationAbility = unit.GetAbility<UnitAnimationAbility>();

            if (unit is AgentUnit agentUnit)
            {
                _baseMoveSpeed = agentUnit.template.MoveSpeed;
            }
            else if (unit is EnemyUnit enemyUnit)
            {
                _baseMoveSpeed = enemyUnit.template.MoveSpeed;
            }
        }

        internal override bool IsExecute()
        {
            return true;
        }

        #region ȸ��
        #region 2D ȸ��
        private bool IsUnitLeft(Vector3 direction)
        {
            Vector3 unitRight = unit.transform.forward;
            float angle = Vector3.SignedAngle(direction, unitRight, Vector3.up);

            return angle > 0f;
        }

        protected void FlipUnit(Vector3 direction)
        {
            bool isLeft = IsUnitLeft(direction);

            float scaleX = isLeft ? 1f : -1f;
            transform.GetChild(3).DOScaleX(scaleX, 0.1f);
        }
        #endregion

        #region 3D ȸ��
        protected void RotateUnit(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2);
            }
        }
        #endregion
        #endregion

        protected void MoveAnimation()
        {
            _unitAnimationAbility.Move(finalMoveSpeed);
        }

        protected void StopMoveAnimation()
        {
            _unitAnimationAbility.Move(0);
        }
    }
}