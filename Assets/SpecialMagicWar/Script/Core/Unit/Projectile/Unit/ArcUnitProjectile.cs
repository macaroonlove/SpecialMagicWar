using FrameWork.Editor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class ArcUnitProjectile : UnitProjectile
    {
        [SerializeField, Label("������ �ִ� ����")] private float _height;
        [SerializeField, Label("�ӵ�")] private float _speed;

        private Vector3 _startPosition;
        private float _totalMoveTime;
        private float _elapsedTime;

        protected void OnEnable()
        {
            _startPosition = transform.position;
            _totalMoveTime = CalculateTotalMoveTime();
            _elapsedTime = 0f;
        }

        /// <summary>
        /// �� �̵� �ð� ���
        /// </summary>
        private float CalculateTotalMoveTime()
        {
            float distance = Vector3.Distance(_startPosition, _target.projectileHitPoint.position);
            return distance / _speed;
        }

        protected override void Move()
        {
            _elapsedTime += Time.deltaTime;
            float time = Mathf.Clamp01(_elapsedTime / _totalMoveTime);

            Vector3 targetPos = _target.projectileHitPoint.position;

            // ���� �̵�
            Vector3 horizontalPos = Vector3.Lerp(_startPosition, targetPos, time);

            // ���� �̵�
            float verticalOffset = Mathf.Sin(time * Mathf.PI) * _height;

            transform.position = new Vector3(horizontalPos.x, horizontalPos.y + verticalOffset, horizontalPos.z);

            // ��ǥ ��ġ ���� �� �浹 ó��
            if (time >= 1f)
            {
                OnCollision();
            }
        }
    }
}