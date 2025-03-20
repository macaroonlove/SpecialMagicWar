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

        #region ��
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

            menu.AddItem(new GUIContent("����"), false, AddAbility, typeof(AttackAbility));
            menu.AddItem(new GUIContent("�ǰ�"), false, AddAbility, typeof(HitAbility));
            menu.AddItem(new GUIContent("ü��"), false, AddAbility, typeof(HealthAbility));
            menu.AddItem(new GUIContent("������ ���"), false, AddAbility, typeof(DamageCalculateAbility));

            menu.AddItem(new GUIContent("����"), false, AddAbility, typeof(BuffAbility));
            menu.AddItem(new GUIContent("�����̻�"), false, AddAbility, typeof(AbnormalStatusAbility));

            menu.AddItem(new GUIContent("��Ƽ�� ��ų"), false, AddAbility, typeof(ActiveSkillAbility));
            menu.AddItem(new GUIContent("�нú� ��ų"), false, AddAbility, typeof(PassiveSkillAbility));

            menu.AddItem(new GUIContent("Ư�� ���� �̵�"), false, AddAbility, typeof(MoveWayPointAbility));

            menu.AddItem(new GUIContent("����ü"), false, AddAbility, typeof(ProjectileAbility));
            menu.AddItem(new GUIContent("��ǥ ã��"), false, AddAbility, typeof(FindTargetAbility));
            menu.AddItem(new GUIContent("FX"), false, AddAbility, typeof(FXAbility));
            menu.AddItem(new GUIContent("�ִϸ��̼�"), false, AddAbility, typeof(UnitAnimationAbility));

            menu.ShowAsContext();
        }
    }
}
#endif
