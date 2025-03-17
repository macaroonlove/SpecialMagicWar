using FrameWork.Editor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class StraightPointProjectile : PointProjectile
    {
        [SerializeField, Label("�ӵ�")] private float _speed;

        protected override void Move()
        {
            float distance = (_targetVector - transform.position).sqrMagnitude;
            float moveDistance = _speed * Time.deltaTime;

            if (distance > moveDistance * moveDistance)
            {
                var dir = (_targetVector - transform.position).normalized;
                var deltaPos = dir * moveDistance;
                transform.Translate(deltaPos);
            }
        }
    }
}