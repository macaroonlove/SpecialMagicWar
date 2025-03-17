using System.Text;

namespace SpecialMagicWar.Core
{
    public class UIBattleStat_MagicResistance : UIBattleStat<int>, IBattleStat
    {
        private DamageCalculateAbility _damageCalculateAbility;
        private BuffAbility _buffAbility;
        private AbnormalStatusAbility _abnormalStatusAbility;

        public override void Initialize(Unit unit)
        {
            base.Initialize(unit);

            _damageCalculateAbility = _unit.GetAbility<DamageCalculateAbility>();
            _buffAbility = _unit.GetAbility<BuffAbility>();
            _abnormalStatusAbility = _unit.GetAbility<AbnormalStatusAbility>();
        }

        public override void Deinitialize()
        {
            base.Deinitialize();

            _damageCalculateAbility = null;
            _buffAbility = null;
            _abnormalStatusAbility = null;
        }

        protected override int GetValue()
        {
            return _damageCalculateAbility.finalMagicResistance;
        }

        protected override string GetBaseValue()
        {
            return $"�⺻ ���� ���׷�: {_damageCalculateAbility.baseMagicResistance}";
        }

        protected override string GetValueText()
        {
            return GetValue().ToString("N0");
        }

        protected override string GetTooltip()
        {
            StringBuilder result = new StringBuilder();

            foreach (var effect in _buffAbility.MagicResistanceAdditionalDataEffects)
            {
                result.AppendLine($"{effect.Value} {ValueFormat(effect.Key.value, EDataType.Add)}");
            }

            foreach (var effect in _buffAbility.MagicResistanceIncreaseDataEffects)
            {
                result.AppendLine($"{effect.Value} {ValueFormat(effect.Key.value, EDataType.Increase)}");
            }
            foreach (var effect in _abnormalStatusAbility.MagicResistanceIncreaseDataEffects)
            {
                result.AppendLine($"{effect.Value} {ValueFormat(effect.Key.value, EDataType.Increase)}");
            }

            foreach (var effect in _buffAbility.MagicResistanceMultiplierDataEffects)
            {
                result.AppendLine($"{effect.Value} {ValueFormat(effect.Key.value, EDataType.Multiplier)}");
            }

            return result.ToString();
        }
    }
}