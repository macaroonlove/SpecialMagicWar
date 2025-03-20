using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class EnemyUnit : Unit
    {
        private EnemyTemplate _template;

        internal EnemyTemplate template => _template;

        internal void Initialize(EnemyTemplate template)
        {
            _id = template.id;
            _template = template;

            base.Initialize(this);
        }

        protected override void OnDeath()
        {
            base.OnDeath();

            if (botIndex > 0)
            {
                BotDeath();
                return;
            }

            if (template.gainCost > 0)
            {
                BattleManager.Instance.GetSubSystem<CostSystem>().AddCost(template.gainCost);
            }
            if (template.gainSoul > 0)
            {
                BattleManager.Instance.GetSubSystem<SoulSystem>().AddSoul(template.gainSoul);
            }
        }

        #region 봇
        internal int botIndex { get; private set; }

        internal void SetBotIndex(int index)
        {
            botIndex = index;

            InitializeBot();
        }

        private void BotDeath()
        {
            if (template.gainCost > 0)
            {
                BattleManager.Instance.GetSubSystem<CostSystem>().AddBotCost(template.gainCost, botIndex);
            }
            if (template.gainSoul > 0)
            {
                BattleManager.Instance.GetSubSystem<SoulSystem>().AddBotSoul(template.gainSoul, botIndex);
            }
        }

        private void InitializeBot()
        {

        }
        #endregion
    }
}

#if UNITY_EDITOR
namespace SpecialMagicWar.Editor
{
    using SpecialMagicWar.Core;
    using UnityEditor;

    [CustomEditor(typeof(EnemyUnit))]
    public class EnemyUnitEditor : UnitEditor
    {
        protected override void AddAbilityMenu()
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("공격"), false, AddAbility, typeof(AttackAbility));
            menu.AddItem(new GUIContent("피격"), false, AddAbility, typeof(HitAbility));
            menu.AddItem(new GUIContent("체력"), false, AddAbility, typeof(HealthAbility));
            menu.AddItem(new GUIContent("데미지 계산"), false, AddAbility, typeof(DamageCalculateAbility));

            menu.AddItem(new GUIContent("버프"), false, AddAbility, typeof(BuffAbility));
            menu.AddItem(new GUIContent("상태이상"), false, AddAbility, typeof(AbnormalStatusAbility));

            menu.AddItem(new GUIContent("액티브 스킬"), false, AddAbility, typeof(ActiveSkillAbility));
            menu.AddItem(new GUIContent("패시브 스킬"), false, AddAbility, typeof(PassiveSkillAbility));

            menu.AddItem(new GUIContent("특정 지점 이동"), false, AddAbility, typeof(MoveWayPointAbility));

            menu.AddItem(new GUIContent("투사체"), false, AddAbility, typeof(ProjectileAbility));
            menu.AddItem(new GUIContent("목표 찾기"), false, AddAbility, typeof(FindTargetAbility));
            menu.AddItem(new GUIContent("FX"), false, AddAbility, typeof(FXAbility));
            menu.AddItem(new GUIContent("애니메이션"), false, AddAbility, typeof(UnitAnimationAbility));

            menu.ShowAsContext();
        }
    }
}
#endif
