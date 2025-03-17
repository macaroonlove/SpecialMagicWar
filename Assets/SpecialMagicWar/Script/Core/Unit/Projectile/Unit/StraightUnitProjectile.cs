using FrameWork.Editor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class StraightUnitProjectile : UnitProjectile
    {
        [SerializeField, Label("�ӵ�")] private float _speed;

        protected override void Move()
        {
            var targetPos = _target.projectileHitPoint.position;
            var distance = (transform.position - targetPos).sqrMagnitude;
            var moveDistance = _speed * Time.deltaTime;

            // ���󰡴� ��
            if (distance > moveDistance * moveDistance)
            {
                var dir = (targetPos - transform.position).normalized;
                var deltaPos = dir * moveDistance;
                transform.Translate(deltaPos);
            }
            // �浹
            else
            {
                OnCollision();
            }
        }
    }
}