using System;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class PhysicalResistanceAdditionalDataEffect : DataEffect<int>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"���� ���׷��� �߰��ϰų� �ٿ��ּ���.";
            }
            else
            {
                return $"���� ���׷�  {value} �߰�";
            }
        }
    }
}