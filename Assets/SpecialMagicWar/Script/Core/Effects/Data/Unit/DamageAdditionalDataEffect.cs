using System;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class DamageAdditionalDataEffect : DataEffect<int>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"���ط��� �߰��ϰų� �ٿ��ּ���.";
            }
            else
            {
                return $"���ط�  {value} �߰�";
            }
        }
    }
}