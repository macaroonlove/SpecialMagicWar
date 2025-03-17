using System;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class HealingIncreaseDataEffect : DataEffect<float>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"ȸ������ ���������� �����ּ���.";
            }
            else if (value > 0)
            {
                return $"ȸ����  {value * 100}% ����";
            }
            else
            {
                return $"ȸ����  {value * 100}% ����";
            }
        }
    }
}