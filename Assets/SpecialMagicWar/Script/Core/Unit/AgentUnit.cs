using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class AgentUnit : Unit
    {
        private AgentTemplate _template;

        internal AgentTemplate template => _template;

        internal void Initialize(AgentTemplate template)
        {
            _id = template.id;
            _template = template;

            base.Initialize(this);
        }
    }
}

#if UNITY_EDITOR
namespace SpecialMagicWar.Editor
{
    using SpecialMagicWar.Core;
    using UnityEditor;

    [CustomEditor(typeof(AgentUnit))]
    public class AgentUnitEditor : UnitEditor
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

            menu.AddItem(new GUIContent("����"), false, AddAbility, typeof(ManaAbility));
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