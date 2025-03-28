using FrameWork.Editor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// 아군 유닛을 관리하는 클래스
    /// (유틸리티 메서드)
    /// </summary>
    public class AgentSystem : MonoBehaviour, IBattleSystem
    {
        [SerializeField, ReadOnly] private List<AgentUnit> _players = new List<AgentUnit>();
        [SerializeField, ReadOnly] private List<(HolyAnimalUnit, int)> _holyAnimals = new List<(HolyAnimalUnit, int)>();

        internal event UnityAction<Unit> onRegist;

        public void Initialize()
        {

        }

        public void Deinitialize()
        {
            // 유닛 오브젝트 모두 파괴
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

        internal void Regist(HolyAnimalUnit holyAnimal, int botIndex)
        {
            _holyAnimals.Add((holyAnimal, botIndex));

            onRegist?.Invoke(holyAnimal);
        }

        internal void Deregist(HolyAnimalUnit holyAnimal, int botIndex)
        {
            _holyAnimals.RemoveAll(item => item.Item1 == holyAnimal && item.Item2 == botIndex);
        }

        #region 유틸리티 메서드
        /// <summary>
        /// 등록된 모든 아군 유닛을 반환
        /// </summary>
        internal List<AgentUnit> GetAllAgents()
        {
            return _players;
        }

        internal List<HolyAnimalUnit> GetAllHolyAnimals(int botIndex = 0)
        {
            return _holyAnimals.Where(item => item.Item2 == botIndex)
                .Select(item => item.Item1).ToList();
        }

        /// <summary>
        /// 범위 내에 아군 유닛을 반환 (unitPos와 가까운 유닛부터 반환)
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
        /// 범위 내, 공격 가능한 아군 유닛을 반환
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
                    // 공격 대상이 아니라면 타겟에 추가하지 않음
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
        /// 유닛의 위치를 기준으로 아래 방향으로 공격 가능한 아군 유닛을 반환
        /// </summary>
        internal List<AgentUnit> GetAttackableAgentsInStraight(Vector3 unitPos, float radius, EAttackType attackType, int maxCount = int.MaxValue)
        {
            List<AgentUnit> agents = new List<AgentUnit>();
            List<(AgentUnit enemy, float distance)> agentsWithDistance = new List<(AgentUnit, float)>();

            foreach (AgentUnit agent in _players)
            {
                if (agent != null && agent.isActiveAndEnabled)
                {
                    // 공격 대상이 아니라면 타겟에 추가하지 않음
                    if (!agent.GetAbility<HitAbility>().finalTargetOfAttack) continue;

                    Vector3 direction = (agent.transform.position - unitPos).normalized;

                    // Y축 아래 방향 (Vector3.down)과 유사한 방향인지 확인
                    if (Vector3.Dot(direction, Vector3.down) >= 0.95f) // 0.95는 약간의 오차 허용
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
        /// 공격 가능한 모든 아군 유닛을 반환
        /// </summary>
        internal List<AgentUnit> GetAllAttackableAgents(EAttackType attackType)
        {
            List<AgentUnit> agents = new List<AgentUnit>();

            foreach (AgentUnit agent in _players)
            {
                if (agent != null && agent.isActiveAndEnabled)
                {
                    // 공격 대상이 아니라면 타겟에 추가하지 않음
                    if (agent.GetAbility<HitAbility>().finalTargetOfAttack == false) continue;

                    agents.Add(agent);
                }
            }

            return agents;
        }

        /// <summary>
        /// 범위 내, 회복 가능한 아군 유닛을 반환
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
                    // 회복 가능 유닛이 아니라면 타겟에 추가하지 않음
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
        /// 회복 가능한 모든 아군 유닛을 반환
        /// </summary>
        internal List<AgentUnit> GetAllHealableAgents()
        {
            List<AgentUnit> agents = new List<AgentUnit>();

            foreach (AgentUnit agent in _players)
            {
                if (agent != null && agent.isActiveAndEnabled)
                {
                    // 회복 가능 유닛이 아니라면 타겟에 추가하지 않음
                    if (agent.GetAbility<HealthAbility>().finalIsHealAble == false) continue;

                    agents.Add(agent);
                }
            }

            return agents;
        }

        /// <summary>
        /// 범위 내에 가장 가까운 아군 유닛을 반환
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