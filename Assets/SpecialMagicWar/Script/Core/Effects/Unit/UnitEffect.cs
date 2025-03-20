namespace SpecialMagicWar.Core
{
    public abstract class UnitEffect : Effect
    {
        public abstract void Execute(Unit casterUnit, Unit targetUnit, ESpellType spellType);
    }
}