using System;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class PhysicalResistanceIncreaseDataEffect : DataEffect<float>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"���� ���׷��� ���������� �����ּ���.";
            }
            else if (value > 0)
            {
                return $"���� ���׷�  {value * 100}% ����";
            }
            else
            {
                return $"���� ���׷�  {value * 100}% ����";
            }
        }
    }
}