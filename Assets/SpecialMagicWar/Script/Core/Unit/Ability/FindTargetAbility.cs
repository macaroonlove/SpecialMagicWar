using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class FindTargetAbility : AlwaysAbility
    {
        private AgentSystem _agentSystem;
        private EnemySystem _enemySystem;

        internal override void Initialize(Unit unit)
        {
            base.Initialize(unit);

            _agentSystem = BattleManager.Instance.GetSubSystem<AgentSystem>();
            _enemySystem = BattleManager.Instance.GetSubSystem<EnemySystem>();
        }

        internal List<Unit> FindAllTarget(EUnitType unitType)
        {
            List<Unit> targets = new List<Unit>();

            switch (unitType)
            {
                case EUnitType.All:
                    targets.AddRange(BattleManager.Instance.GetSubSystem<AgentSystem>().GetAllAgents());
                    targets.AddRange(BattleManager.Instance.GetSubSystem<EnemySystem>().GetAllEnemies());
                    break;
                case EUnitType.Agent:
                    targets.AddRange(BattleManager.Instance.GetSubSystem<AgentSystem>().GetAllAgents());
                    break;
                case EUnitType.Enemy:
                    targets.AddRange(BattleManager.Instance.GetSubSystem<EnemySystem>().GetAllEnemies());
                    break;
            }

            return targets;
        }

        /// <summary>
        /// ���� ������ ���� ã��
        /// </summary>
        internal List<Unit> FindAttackableTarget(ETarget target, float range, EAttackType attackType, ESkillRangeType rangeType, int attackCount = int.MaxValue)
        {
            List<Unit> targets = new List<Unit>();

            if (unit is AgentUnit || unit is HolyAnimalUnit)
            {
                List<EnemyUnit> attackTargets = new List<EnemyUnit>();

                switch (target)
                {
                    case ETarget.Myself:
                        targets.Add(unit);
                        return targets;
                    case ETarget.OneTargetInRange:
                        if (rangeType == ESkillRangeType.Circle)
                        {
                            attackTargets = _enemySystem.GetAttackableEnemiesInRadius(transform.position, range, attackType, 1);
                        }
                        else if (rangeType == ESkillRangeType.Straight)
                        {
                            attackTargets = _enemySystem.GetAttackableEnemiesInStraight(transform.position, range, attackType, 1);
                        }
                        break;
                    case ETarget.NumTargetInRange:
                        if (rangeType == ESkillRangeType.Circle)
                        {
                            attackTargets = _enemySystem.GetAttackableEnemiesInRadius(transform.position, range, attackType, attackCount);
                        }
                        else if (rangeType == ESkillRangeType.Straight)
                        {
                            attackTargets = _enemySystem.GetAttackableEnemiesInStraight(transform.position, range, attackType, attackCount);
                        }
                        break;
                    case ETarget.AllTargetInRange:
                        if (rangeType == ESkillRangeType.Circle)
                        {
                            attackTargets = _enemySystem.GetAttackableEnemiesInRadius(transform.position, range, attackType);
                        }
                        else if (rangeType == ESkillRangeType.Straight)
                        {
                            attackTargets = _enemySystem.GetAttackableEnemiesInStraight(transform.position, range, attackType);
                        }
                        break;
                    case ETarget.AllTarget:
                        attackTargets = _enemySystem.GetAttackableAllEnemies(attackType);
                        break;
                }

                if (attackTargets.Count > 0)
                {
                    targets.AddRange(attackTargets);
                }
            }
            else if (unit is EnemyUnit)
            {
                List<AgentUnit> attackTargets = new List<AgentUnit>();

                switch (target)
                {
                    case ETarget.Myself:
                        targets.Add(unit);
                        return targets;
                    case ETarget.OneTargetInRange:
                        if (rangeType == ESkillRangeType.Circle)
                        {
                            attackTargets = _agentSystem.GetAttackableAgentsInRadius(transform.position, range, attackType, 1);
                        }
                        else if (rangeType == ESkillRangeType.Straight)
                        {
                            attackTargets = _agentSystem.GetAttackableAgentsInStraight(transform.position, range, attackType, 1);
                        }
                        break;
                    case ETarget.NumTargetInRange:
                        if (rangeType == ESkillRangeType.Circle)
                        {
                            attackTargets = _agentSystem.GetAttackableAgentsInRadius(transform.position, range, attackType);
                        }
                        else if (rangeType == ESkillRangeType.Straight)
                        {
                            attackTargets = _agentSystem.GetAttackableAgentsInStraight(transform.position, range, attackType);
                        }
                        break;
                    case ETarget.AllTargetInRange:
                        if (rangeType == ESkillRangeType.Circle)
                        {
                            attackTargets = _agentSystem.GetAttackableAgentsInRadius(transform.position, range, attackType);
                        }
                        else if (rangeType == ESkillRangeType.Straight)
                        {
                            attackTargets = _agentSystem.GetAttackableAgentsInStraight(transform.position, range, attackType);
                        }
                        break;
                    case ETarget.AllTarget:
                        attackTargets = _agentSystem.GetAllAttackableAgents(attackType);
                        break;
                }

                if (attackTargets.Count > 0)
                {
                    targets.AddRange(attackTargets);
                }
            }

            return targets;
        }

        /// <summary>
        /// ȸ�� ������ ���� ã��
        /// </summary>
        internal List<Unit> FindHealableTarget(ETarget target, float range, int healCount = int.MaxValue)
        {
            List<Unit> targets = new List<Unit>();

            if (unit is AgentUnit || unit is HolyAnimalUnit)
            {
                List<AgentUnit> healTargets = new List<AgentUnit>();

                switch (target)
                {
                    case ETarget.Myself:
                        targets.Add(unit);
                        return targets;
                    case ETarget.OneTargetInRange:
                        healTargets = _agentSystem.GetHealableAgentsInRadius(transform.position, range, 1);
                        break;
                    case ETarget.NumTargetInRange:
                        healTargets = _agentSystem.GetHealableAgentsInRadius(transform.position, range, healCount);
                        break;
                    case ETarget.AllTargetInRange:
                        healTargets = _agentSystem.GetHealableAgentsInRadius(transform.position, range);
                        break;
                    case ETarget.AllTarget:
                        healTargets = _agentSystem.GetAllHealableAgents();
                        break;
                }

                if (healTargets.Count > 0)
                {
                    targets.AddRange(healTargets);
                }
            }
            else if (unit is EnemyUnit)
            {
                List<EnemyUnit> healTargets = new List<EnemyUnit>();

                switch (target)
                {
                    case ETarget.Myself:
                        targets.Add(unit);
                        return targets;
                    case ETarget.OneTargetInRange:
                        healTargets = _enemySystem.GetHealableEnemiesInRadius(transform.position, range, 1);
                        break;
                    case ETarget.NumTargetInRange:
                        healTargets = _enemySystem.GetHealableEnemiesInRadius(transform.position, range, healCount);
                        break;
                    case ETarget.AllTargetInRange:
                        healTargets = _enemySystem.GetHealableEnemiesInRadius(transform.position, range);
                        break;
                    case ETarget.AllTarget:
                        healTargets = _enemySystem.GetHealableAllEnemies();
                        break;
                }

                if (healTargets.Count > 0)
                {
                    targets.AddRange(healTargets);
                }
            }

            return targets;
        }

        /// <summary>
        /// �Ʊ� ���� ã��
        /// (AgentUnit�� ��� AgentUnit�� ��ȯ�ϰ�, EnemyUnit�� ��� EnemyUnit�� ��ȯ�Ѵ�.)
        /// </summary>
        internal List<Unit> FindAllyTarget(ETarget target, float range, int healCount = int.MaxValue)
        {
            List<Unit> targets = new List<Unit>();

            if (unit is AgentUnit || unit is HolyAnimalUnit)
            {
                List<AgentUnit> allyTargets = new List<AgentUnit>();

                switch (target)
                {
                    case ETarget.Myself:
                        targets.Add(unit);
                        return targets;
                    case ETarget.OneTargetInRange:
                        allyTargets = _agentSystem.GetAgentsInRadius(transform.position, range, 1);
                        break;
                    case ETarget.NumTargetInRange:
                        allyTargets = _agentSystem.GetAgentsInRadius(transform.position, range, healCount);
                        break;
                    case ETarget.AllTargetInRange:
                        allyTargets = _agentSystem.GetAgentsInRadius(transform.position, range);
                        break;
                    case ETarget.AllTarget:
                        allyTargets = _agentSystem.GetAllAgents();
                        break;
                }

                if (allyTargets.Count > 0)
                {
                    targets.AddRange(allyTargets);
                }
            }
            else if (unit is EnemyUnit)
            {
                List<EnemyUnit> allyTargets = new List<EnemyUnit>();

                switch (target)
                {
                    case ETarget.Myself:
                        targets.Add(unit);
                        return targets;
                    case ETarget.OneTargetInRange:
                        allyTargets = _enemySystem.GetEnemiesInRadius(transform.position, range, 1);
                        break;
                    case ETarget.NumTargetInRange:
                        allyTargets = _enemySystem.GetEnemiesInRadius(transform.position, range, healCount);
                        break;
                    case ETarget.AllTargetInRange:
                        allyTargets = _enemySystem.GetEnemiesInRadius(transform.position, range);
                        break;
                    case ETarget.AllTarget:
                        allyTargets = _enemySystem.GetAllEnemies();
                        break;
                }

                if (allyTargets.Count > 0)
                {
                    targets.AddRange(allyTargets);
                }
            }

            return targets;
        }
    }
}
