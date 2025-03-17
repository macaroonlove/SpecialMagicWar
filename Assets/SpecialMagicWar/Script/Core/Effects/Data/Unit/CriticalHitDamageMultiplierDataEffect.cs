using System;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class CriticalHitDamageMultiplierDataEffect : DataEffect<float>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"ġ��Ÿ �������� ��¡��϶� �����ּ���.";
            }
            else if (value > 0)
            {
                return $"ġ��Ÿ ������  {value * 100}% ���";
            }
            else
            {
                return $"ġ��Ÿ ������  {value * 100}% �϶�";
            }
        }
    }
}