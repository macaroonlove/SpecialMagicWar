using UnityEngine;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// Ư�� �������� ȿ���� ����
    /// </summary>
    public abstract class PointEffect : Effect
    {
        public abstract void Execute(Unit casterUnit, ESpellType spellType);
    }
}