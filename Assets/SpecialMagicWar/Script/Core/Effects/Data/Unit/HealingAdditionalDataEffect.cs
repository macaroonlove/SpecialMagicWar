using System;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class HealingAdditionalDataEffect : DataEffect<int>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"ȸ������ �߰��ϰų� �ٿ��ּ���.";
            }
            else
            {
                return $"ȸ����  {value} �߰�";
            }
        }
    }
}