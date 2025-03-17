using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    public class ActiveItemRangeRenderer : MonoBehaviour, IBattleSystem
    {
        private Transform _parent;
        private Transform _circle;

        private AgentSystem _agentSystem;
        private EnemySystem _enemySystem;

        private Camera _camera;
        private Plane _plane;

        private ERangeType _rangeType;
        private EUnitType _unitType;
        private float _radius;

        private bool _isShowRenderer;

        private UnityAction<List<Unit>> _action;

        public void Initialize()
        {
            _agentSystem = BattleManager.Instance.GetSubSystem<AgentSystem>();
            _enemySystem = BattleManager.Instance.GetSubSystem<EnemySystem>();

            _camera = Camera.main;
            _plane = new Plane(Vector3.up, new Vector3(0, 0.3f, 0));

            _parent = transform.GetChild(0);
            _circle = _parent.GetChild(0);

            Hide();
        }

        public void Deinitialize()
        {
            _agentSystem = null;
            _enemySystem = null;
        }

        /// <summary>
        /// Ÿ�Կ� �´� ��� ������ ��ȯ
        /// </summary>
        internal void Show_AllRange(EUnitType unitType, float delay, UnityAction<List<Unit>> action, UnityAction<Vector3> processAction)
        {
            _rangeType = ERangeType.All;
            _unitType = unitType;
            _action = action;

            var units = GetUnits_AllRange();

            if (units.Count > 0)
            {
                StartCoroutine(CoConfirm(delay, units));

                // TODO: ���� �߾� or ȭ���� �߾ӿ� �ִ� ���� ��ȯ�ϵ��� ����
                processAction?.Invoke(Vector3.zero);
            }
        }

        /// <summary>
        /// Ÿ�Կ� �´� ��� ������ ��ȯ
        /// </summary>
        internal void Show_CircleRange(EUnitType unitType, float radius, UnityAction<List<Unit>> action)
        {
            _rangeType = ERangeType.Circle;
            _unitType = unitType;
            _radius = radius;
            _action = action;

            _circle.localScale = radius * Vector3.one;
            _circle.gameObject.SetActive(true);

            _isShowRenderer = true;

            // TODO: �ð� ����?
        }

        /// <summary>
        /// ���� Ȯ���ϱ�
        /// </summary>
        internal void Confirm(float delay, UnityAction<Vector3> processAction)
        {
            var units = GetUnits_CircleRange(_parent.position);

            if (units.Count > 0)
            {
                StartCoroutine(CoConfirm(delay, units));

                processAction?.Invoke(_parent.position);
            }

            Hide();
        }

        private IEnumerator CoConfirm(float delay, List<Unit> units)
        {
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            _action?.Invoke(units);
        }

        private void Update()
        {
            if (_isShowRenderer == false) return;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (_plane.Raycast(ray, out float distance))
            {
                _parent.position = ray.GetPoint(distance);
            }
        }

        private void Hide()
        {
            _isShowRenderer = false;
            _circle.gameObject.SetActive(false);
        }

        #region ���� ��������
        private List<Unit> GetUnits_AllRange()
        {
            List<Unit> units = new List<Unit>();

            switch (_unitType)
            {
                case EUnitType.All:
                    units.AddRange(_agentSystem.GetAllAgents());
                    units.AddRange(_enemySystem.GetAllEnemies());
                    break;
                case EUnitType.Agent:
                    units.AddRange(_agentSystem.GetAllAgents());
                    break;
                case EUnitType.Enemy:
                    units.AddRange(_enemySystem.GetAllEnemies());
                    break;
            }

            return units;
        }

        private List<Unit> GetUnits_CircleRange(Vector3 pos)
        {
            List<Unit> units = new List<Unit>();

            switch (_unitType)
            {
                case EUnitType.All:
                    units.AddRange(_agentSystem.GetAgentsInRadius(pos, _radius));
                    units.AddRange(_enemySystem.GetEnemiesInRadius(pos, _radius));
                    break;
                case EUnitType.Agent:
                    units.AddRange(_agentSystem.GetAgentsInRadius(pos, _radius));
                    break;
                case EUnitType.Enemy:
                    units.AddRange(_enemySystem.GetEnemiesInRadius(pos, _radius));
                    break;
            }

            return units;
        }
        #endregion
    }
}
