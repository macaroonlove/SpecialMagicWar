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

        protected override void OnDeath()
        {
            base.OnDeath();

            if (botIndex == 0)
            {
                // �й� ��ȣ ������
                BattleManager.Instance.DefeatBattle();
            }

            // ������ ���ܽ�Ű��
            BattleManager.Instance.deadBot.Add(botIndex);

            // �ż� ����
            var agentSystem = BattleManager.Instance.GetSubSystem<AgentSystem>();
            var holyAnimals = agentSystem.GetAllHolyAnimals(botIndex);
            for (int i = holyAnimals.Count - 1; i >= 0; i--)
            {
                CoreManager.Instance.GetSubSystem<PoolSystem>().DeSpawn(holyAnimals[i].gameObject, 1);
                agentSystem.Deregist(holyAnimals[i], botIndex);
            }

            // �� ����
            var enemySystem = BattleManager.Instance.GetSubSystem<EnemySystem>();
            var enemies = enemySystem.GetAllEnemies();
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (enemies[i].botIndex == botIndex)
                {
                    CoreManager.Instance.GetSubSystem<PoolSystem>().DeSpawn(enemies[i].gameObject, 1);
                    enemySystem.Deregist(enemies[i]);
                }
            }
        }

        #region ��
        internal int botIndex { get; private set; }

        internal void SetBotIndex(int index)
        {
            botIndex = index;

            InitializeBot();
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

            menu.AddItem(new GUIContent("��Ƽ�� ��ų"), false, AddAbility, typeof(ActiveSkillAbility));
            menu.AddItem(new GUIContent("�нú� ��ų"), false, AddAbility, typeof(PassiveSkillAbility));

            menu.AddItem(new GUIContent("����ü"), false, AddAbility, typeof(ProjectileAbility));
            menu.AddItem(new GUIContent("��ǥ ã��"), false, AddAbility, typeof(FindTargetAbility));
            menu.AddItem(new GUIContent("FX"), false, AddAbility, typeof(FXAbility));
            menu.AddItem(new GUIContent("�ִϸ��̼�"), false, AddAbility, typeof(UnitAnimationAbility));

            menu.ShowAsContext();
        }
    }
}
#endif