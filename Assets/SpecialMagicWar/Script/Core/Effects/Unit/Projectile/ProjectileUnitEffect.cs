using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public abstract class ProjectileUnitEffect : UnitEffect
    {
        [SerializeField] protected GameObject _prefab;
        [SerializeField] protected ESpawnPoint _spawnPoint;

        public override void Execute(Unit casterUnit, Unit targetUnit)
        {
            if (casterUnit == null || targetUnit == null) return;
            if (targetUnit.isDie) return;

            casterUnit.GetAbility<ProjectileAbility>().SpawnProjectile(_prefab, _spawnPoint, targetUnit, (caster, target) => { SkillImpact(caster, target); });
        }

        protected abstract void SkillImpact(Unit casterUnit, Unit targetUnit);

#if UNITY_EDITOR
        protected float lastRectY { get; private set; }

        public override void Draw(Rect rect)
        {
            var labelRect = new Rect(rect.x, rect.y, 140, rect.height);
            var valueRect = new Rect(rect.x + 140, rect.y, rect.width - 140, rect.height);

            GUI.Label(labelRect, "������");
            _prefab = (GameObject)EditorGUI.ObjectField(valueRect, _prefab, typeof(GameObject), false);

            labelRect.y += 20;
            valueRect.y += 20;
            GUI.Label(labelRect, "����ü ���� ��ġ");
            _spawnPoint = (ESpawnPoint)EditorGUI.EnumPopup(valueRect, _spawnPoint);

            lastRectY = labelRect.y;
        }

        public override int GetNumRows()
        {
            return 2;
        }
#endif
    }
}