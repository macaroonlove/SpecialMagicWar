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
                // 패배 신호 보내기
                BattleManager.Instance.DefeatBattle();
            }

            // 봇에서 제외시키기
            BattleManager.Instance.deadBot.Add(botIndex);

            // 신수 디스폰
            var agentSystem = BattleManager.Instance.GetSubSystem<AgentSystem>();
            var holyAnimals = agentSystem.GetAllHolyAnimals(botIndex);
            for (int i = holyAnimals.Count - 1; i >= 0; i--)
            {
                CoreManager.Instance.GetSubSystem<PoolSystem>().DeSpawn(holyAnimals[i].gameObject, 1);
                agentSystem.Deregist(holyAnimals[i], botIndex);
            }

            // 적 디스폰
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

        #region 봇
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

            menu.AddItem(new GUIContent("공격"), false, AddAbility, typeof(AttackAbility));
            menu.AddItem(new GUIContent("피격"), false, AddAbility, typeof(HitAbility));
            menu.AddItem(new GUIContent("체력"), false, AddAbility, typeof(HealthAbility));
            menu.AddItem(new GUIContent("데미지 계산"), false, AddAbility, typeof(DamageCalculateAbility));

            menu.AddItem(new GUIContent("버프"), false, AddAbility, typeof(BuffAbility));
            menu.AddItem(new GUIContent("상태이상"), false, AddAbility, typeof(AbnormalStatusAbility));

            menu.AddItem(new GUIContent("액티브 스킬"), false, AddAbility, typeof(ActiveSkillAbility));
            menu.AddItem(new GUIContent("패시브 스킬"), false, AddAbility, typeof(PassiveSkillAbility));

            menu.AddItem(new GUIContent("투사체"), false, AddAbility, typeof(ProjectileAbility));
            menu.AddItem(new GUIContent("목표 찾기"), false, AddAbility, typeof(FindTargetAbility));
            menu.AddItem(new GUIContent("FX"), false, AddAbility, typeof(FXAbility));
            menu.AddItem(new GUIContent("애니메이션"), false, AddAbility, typeof(UnitAnimationAbility));

            menu.ShowAsContext();
        }
    }
}
#endif