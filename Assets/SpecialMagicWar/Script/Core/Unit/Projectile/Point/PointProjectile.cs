using FrameWork.Editor;
using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    public abstract class PointProjectile : Projectile
    {
        [SerializeField, Label("�浹 ���")] private LayerMask _layerMask;
        [SerializeField, Label("���� ����")] private bool _isPiercing;

        protected Unit _caster;
        protected Vector3 _targetVector;

        internal virtual void Initialize(Unit caster, Vector3 targetVector, UnityAction<Unit, Unit> action)
        {
            _caster = caster;
            _targetVector = targetVector;
            _action = action;

            ExecuteCasterFX(caster);

            if (_isLookTarget)
            {
                transform.GetChild(0).LookAt(_targetVector);
            }

            _isInit = true;
        }

        private void Update()
        {
            if (_isInit == false) return;

            Move();

            // ���� ��ġ�� �����ߴٸ�
            if ((transform.position - _targetVector).sqrMagnitude < 0.01f)
            {
                DeSpawn();
            }
        }

        protected abstract void Move();

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _layerMask) != 0)
            {
                if (other.TryGetComponent(out Unit targetUnit))
                {
                    OnCollision(targetUnit);
                }
            }
        }

        private void OnCollision(Unit target)
        {
            _action?.Invoke(_caster, target);

            ExecuteTargetFX(target);

            // ����Ǵ� ����ü�� �ƴ϶��
            if (_isPiercing == false)
            {
                DeSpawn();
            }
        }
    }
}