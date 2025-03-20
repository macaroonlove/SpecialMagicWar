using FrameWork.Editor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// �Ʊ� ������ �����ϴ� Ŭ����
    /// (��ƿ��Ƽ �޼���)
    /// </summary>
    public class AgentSystem : MonoBehaviour, IBattleSystem
    {
        [SerializeField, ReadOnly] private List<AgentUnit> _players = new List<AgentUnit>();
        [SerializeField, ReadOnly] private List<HolyAnimalUnit> _holyAnimals = new List<HolyAnimalUnit>();

        internal int holyAnimalCount => _holyAnimals.Count;

        internal event UnityAction<Unit> onRegist;

        public void Initialize()
        {

        }

        public void Deinitialize()
        {
            // ���� ������Ʈ ��� �ı�
            foreach (var agent in _players)
            {
                Destroy(agent.gameObject);
            }
        }

        internal void Regist(AgentUnit agent)
        {
            _players.Add(agent);

            onRegist?.Invoke(agent);
        }

        internal void Deregist(AgentUnit agent)
        {
            _players.Remove(agent);
        }

        internal void Regist(HolyAnimalUnit holyAnimal)
        {
            _holyAnimals.Add(holyAnimal);

            onRegist?.Invoke(holyAnimal);
        }

        internal void Deregist(HolyAnimalUnit holyAnimal)
        {
            _holyAnimals.Remove(holyAnimal);
        }

        #region ��ƿ��Ƽ �޼���
        /// <summary>
        /// ��ϵ� ��� �Ʊ� ������ ��ȯ
        /// </summary>
        internal List<AgentUnit> GetAllAgents()
        {
            return _players;
        }

        internal List<HolyAnimalUnit> GetAllHolyAnimals()
        {
            return _holyAnimals;
        }

        /// <summary>
        /// ���� ���� �Ʊ� ������ ��ȯ (unitPos�� ����� ���ֺ��� ��ȯ)
        /// </summary>
        internal List<AgentUnit> GetAgentsInRadius(Vector3 unitPos, float radius, int maxCount = int.MaxValue)
        {
            List<AgentUnit> agents = new List<AgentUnit>();
            List<(AgentUnit agent, float distance)> agentsWithDistance = new List<(AgentUnit, float)>();

            radius *= radius;

            foreach (AgentUnit agent in _players)
            {
                if (agent != null && agent.isActiveAndEnabled)
                {
                    var distance = (agent.transform.position - unitPos).sqrMagnitude;

                    if (distance <= radius)
                    {
                        agentsWithDistance.Add((agent, distance));
                    }
                }
            }

            if (agentsWithDistance.Count > maxCount)
            {
                agentsWithDistance.Sort((a, b) => a.distance.CompareTo(b.distance));
            }

            foreach (var (agent, _) in agentsWithDistance)
            {
                if (agents.Count >= maxCount) break;
                agents.Add(agent);
            }

            return agents;
        }

        /// <summary>
        /// ���� ��, ���� ������ �Ʊ� ������ ��ȯ
        /// </summary>
        internal List<AgentUnit> GetAttackableAgentsInRadius(Vector3 unitPos, float radius, EAttackType attackType, int maxCount = int.MaxValue)
        {
            List<AgentUnit> agents = new List<AgentUnit>();
            List<(AgentUnit agent, float distance)> agentsWithDistance = new List<(AgentUnit, float)>();

            radius *= radius;

            foreach (AgentUnit agent in _players)
            {
                if (agent != null && agent.isActiveAndEnabled)
                {
                    // ���� ����� �ƴ϶�� Ÿ�ٿ� �߰����� ����
                    if (agent.GetAbility<HitAbility>().finalTargetOfAttack == false) continue;

                    var distance = (agent.transform.position - unitPos).sqrMagnitude;

                    if (distance <= radius)
                    {
                        agentsWithDistance.Add((agent, distance));
                    }
                }
            }

            if (agentsWithDistance.Count > maxCount)
            {
                agentsWithDistance.Sort((a, b) => a.distance.CompareTo(b.distance));
            }

            foreach (var (agent, _) in agentsWithDistance)
            {
                if (agents.Count >= maxCount) break;
                agents.Add(agent);
            }

            return agents;
        }

        /// <summary>
        /// ������ ��ġ�� �������� �Ʒ� �������� ���� ������ �Ʊ� ������ ��ȯ
        /// </summary>
        internal List<AgentUnit> GetAttackableAgentsInStraight(Vector3 unitPos, float radius, EAttackType attackType, int maxCount = int.MaxValue)
        {
            List<AgentUnit> agents = new List<AgentUnit>();
            List<(AgentUnit enemy, float distance)> agentsWithDistance = new List<(AgentUnit, float)>();

            foreach (AgentUnit agent in _players)
            {
                if (agent != null && agent.isActiveAndEnabled)
                {
                    // ���� ����� �ƴ϶�� Ÿ�ٿ� �߰����� ����
                    if (!agent.GetAbility<HitAbility>().finalTargetOfAttack) continue;

                    Vector3 direction = (agent.transform.position - unitPos).normalized;

                    // Y�� �Ʒ� ���� (Vector3.down)�� ������ �������� Ȯ��
                    if (Vector3.Dot(direction, Vector3.down) >= 0.95f) // 0.95�� �ణ�� ���� ���
                    {
                        float distance = Mathf.Abs(agent.transform.position.y - unitPos.y);

                        if (distance <= radius)
                        {
                            agentsWithDistance.Add((agent, distance));
                        }
                    }
                }
            }

            if (agentsWithDistance.Count > maxCount)
            {
                agentsWithDistance.Sort((a, b) => a.distance.CompareTo(b.distance));
            }

            foreach (var (agent, _) in agentsWithDistance)
            {
                if (agents.Count >= maxCount) break;
                agents.Add(agent);
            }

            return agents;
        }

        /// <summary>
        /// ���� ������ ��� �Ʊ� ������ ��ȯ
        /// </summary>
        internal List<AgentUnit> GetAllAttackableAgents(EAttackType attackType)
        {
            List<AgentUnit> agents = new List<AgentUnit>();

            foreach (AgentUnit agent in _players)
            {
                if (agent != null && agent.isActiveAndEnabled)
                {
                    // ���� ����� �ƴ϶�� Ÿ�ٿ� �߰����� ����
                    if (agent.GetAbility<HitAbility>().finalTargetOfAttack == false) continue;

                    agents.Add(agent);
                }
            }

            return agents;
        }

        /// <summary>
        /// ���� ��, ȸ�� ������ �Ʊ� ������ ��ȯ
        /// </summary>
        internal List<AgentUnit> GetHealableAgentsInRadius(Vector3 unitPos, float radius, int maxCount = int.MaxValue)
        {
            List<AgentUnit> agents = new List<AgentUnit>();
            List<(AgentUnit agent, float distance)> agentsWithDistance = new List<(AgentUnit, float)>();

            radius *= radius;

            foreach (AgentUnit agent in _players)
            {
                if (agent != null && agent.isActiveAndEnabled)
                {
                    // ȸ�� ���� ������ �ƴ϶�� Ÿ�ٿ� �߰����� ����
                    if (agent.GetAbility<HealthAbility>().finalIsHealAble == false) continue;

                    var distance = (agent.transform.position - unitPos).sqrMagnitude;

                    if (distance <= radius)
                    {
                        agentsWithDistance.Add((agent, distance));
                    }
                }
            }

            if (agentsWithDistance.Count > maxCount)
            {
                agentsWithDistance.Sort((a, b) => a.distance.CompareTo(b.distance));
            }

            foreach (var (agent, _) in agentsWithDistance)
            {
                if (agents.Count >= maxCount) break;
                agents.Add(agent);
            }

            return agents;
        }

        /// <summary>
        /// ȸ�� ������ ��� �Ʊ� ������ ��ȯ
        /// </summary>
        internal List<AgentUnit> GetAllHealableAgents()
        {
            List<AgentUnit> agents = new List<AgentUnit>();

            foreach (AgentUnit agent in _players)
            {
                if (agent != null && agent.isActiveAndEnabled)
                {
                    // ȸ�� ���� ������ �ƴ϶�� Ÿ�ٿ� �߰����� ����
                    if (agent.GetAbility<HealthAbility>().finalIsHealAble == false) continue;

                    agents.Add(agent);
                }
            }

            return agents;
        }

        /// <summary>
        /// ���� ���� ���� ����� �Ʊ� ������ ��ȯ
        /// </summary>
        internal AgentUnit GetNearestAgent(Vector3 unitPos, float radius)
        {
            AgentUnit agentUnit = null;
            radius *= radius;
            float nearestDistance = Mathf.Infinity;

            foreach (AgentUnit agent in _players)
            {
                if (agent != null && agent.isActiveAndEnabled)
                {
                    float distance = (agent.transform.position - unitPos).sqrMagnitude;

                    if (distance < nearestDistance && distance <= radius)
                    {
                        agentUnit = agent;
                        nearestDistance = distance;
                    }
                }
            }

            return agentUnit;
        }
        #endregion
    }
}