using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public abstract class ProjectilePointEffect : PointEffect
    {
        [SerializeField] protected GameObject _prefab;
        [SerializeField] protected ESpawnPoint _spawnPoint;

        [SerializeField] protected float _skillRange;

        public override void Execute(Unit casterUnit, Vector3 targetVector)
        {
            if (casterUnit == null) return;

            SpawnStraightProjectiles(casterUnit, targetVector);
        }

        private void SpawnStraightProjectiles(Unit casterUnit, Vector3 targetVector)
        {
            Vector3 finalPosition = casterUnit.transform.position + Vector3.down * _skillRange;

            SpawnProjectile(casterUnit, finalPosition);
        }
        
        private void SpawnProjectile(Unit casterUnit, Vector3 finalPosition)
        {
            casterUnit.GetAbility<ProjectileAbility>().SpawnProjectile(_prefab, _spawnPoint, finalPosition, (caster, target) => { SkillImpact(caster, target); });
        }

        protected abstract void SkillImpact(Unit casterUnit, Unit targetUnit);

#if UNITY_EDITOR
        protected float lastRectY { get; private set; }

        public override void Draw(Rect rect)
        {
            var labelRect = new Rect(rect.x, rect.y, 140, rect.height);
            var valueRect = new Rect(rect.x + 140, rect.y, rect.width - 140, rect.height);

            GUI.Label(labelRect, "투사체");
            _prefab = (GameObject)EditorGUI.ObjectField(valueRect, _prefab, typeof(GameObject), false);

            labelRect.y += 20;
            valueRect.y += 20;
            GUI.Label(labelRect, "투사체 생성 위치");
            _spawnPoint = (ESpawnPoint)EditorGUI.EnumPopup(valueRect, _spawnPoint);

            labelRect.y += 40;
            valueRect.y += 40;
            GUI.Label(labelRect, "범위");
            _skillRange = EditorGUI.FloatField(valueRect, _skillRange);


            lastRectY = labelRect.y;
        }

        public override int GetNumRows()
        {
            int rowNum = 5;

            return rowNum;
        }
#endif
    }
}
