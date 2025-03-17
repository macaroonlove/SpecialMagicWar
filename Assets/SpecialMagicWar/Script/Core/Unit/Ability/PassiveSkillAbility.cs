using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// ������ �нú� ��ų ����� �����մϴ�.
    /// </summary>
    public class PassiveSkillAbility : AlwaysAbility
    {
        #region Effect List
        private List<UnitEffect> _attackEventEffects = new List<UnitEffect>();
        private List<UnitEffect> _hitEventEffects = new List<UnitEffect>();
        private List<UnitEffect> _healEventEffects = new List<UnitEffect>();
        private List<UnitEffect> _destroyShieldEventEffects = new List<UnitEffect>();

        #region ������Ƽ
        internal IReadOnlyList<UnitEffect> attackEventEffects => _attackEventEffects;
        internal IReadOnlyList<UnitEffect> hitEventEffects => _hitEventEffects;
        internal IReadOnlyList<UnitEffect> healEventEffects => _healEventEffects;
        internal IReadOnlyList<UnitEffect> destroyShieldEventEffects => _destroyShieldEventEffects;
        #endregion
        #endregion

        //internal override void Initialize(Unit unit)
        //{
        //    base.Initialize(unit);

        //    if (unit is AgentUnit agentUnit)
        //    {
        //        InitializePassiveSkill(agentUnit.template.skillTreeGraph);
        //    }
        //    else if (unit is EnemyUnit enemyUnit)
        //    {
        //        InitializePassiveSkill(enemyUnit.template.skillTreeGraph);
        //    }
        //}

        //private void InitializePassiveSkill(SkillTreeGraph skillTree)
        //{
        //    if (skillTree == null) return;

        //    foreach (var node in skillTree.nodes)
        //    {
        //        if (node is PassiveSkillNode skill)
        //        {
        //            foreach (var trigger in skill.skillTemplate.triggers)
        //            {
        //                // ��� ����� ȿ��
        //                if (trigger is AlwaysUnitTrigger alwaysUnitTrigger)
        //                {
        //                    foreach (var effect in alwaysUnitTrigger.effects)
        //                    {
        //                        if (effect is AlwaysEffect alwaysEffect)
        //                        {
        //                            alwaysEffect.Execute(unit);
        //                        }
        //                    }
        //                }
        //                // �⺻ ����/ȸ�� �� ����� ȿ��
        //                else if (trigger is AttackEventUnitTrigger attackEventUnitTrigger)
        //                {
        //                    foreach (var effect in attackEventUnitTrigger.effects)
        //                    {
        //                        if (effect is UnitEffect eventEffect)
        //                        {
        //                            _attackEventEffects.Add(eventEffect);
        //                        }
        //                    }
        //                }
        //                // �ǰ� �� ����� ȿ��
        //                else if (trigger is HitEventUnitTrigger hitEventUnitTrigger)
        //                {
        //                    foreach (var effect in hitEventUnitTrigger.effects)
        //                    {
        //                        if (effect is UnitEffect eventEffect)
        //                        {
        //                            _hitEventEffects.Add(eventEffect);
        //                        }
        //                    }
        //                }
        //                // ȸ���� ���� �� ����� ȿ��
        //                else if (trigger is HealEventUnitTrigger healEventUnitTrigger)
        //                {
        //                    foreach (var effect in healEventUnitTrigger.effects)
        //                    {
        //                        if (effect is UnitEffect unitEffect)
        //                        {
        //                            _healEventEffects.Add(unitEffect);
        //                        }
        //                    }
        //                }
        //                // ��ȣ���� �ı��� �� ����� ȿ��
        //                else if (trigger is DestroyShieldEventUnitTrigger destroyShieldEventUnitTrigger)
        //                {
        //                    foreach (var effect in destroyShieldEventUnitTrigger.effects)
        //                    {
        //                        if (effect is UnitEffect unitEffect)
        //                        {
        //                            _destroyShieldEventEffects.Add(unitEffect);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        internal override void Deinitialize()
        {
            _attackEventEffects.Clear();
            _hitEventEffects.Clear();
            _healEventEffects.Clear();
            _destroyShieldEventEffects.Clear();
        }
    }
}