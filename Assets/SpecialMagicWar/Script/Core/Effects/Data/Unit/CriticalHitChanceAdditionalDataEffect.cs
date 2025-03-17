using System;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class CriticalHitChanceAdditionalDataEffect : DataEffect<int>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"ġ��Ÿ Ȯ���� �߰��ϰų� �ٿ��ּ���.";
            }
            else
            {
                return $"ġ��Ÿ Ȯ��  {value} �߰�";
            }
        }
    }
}