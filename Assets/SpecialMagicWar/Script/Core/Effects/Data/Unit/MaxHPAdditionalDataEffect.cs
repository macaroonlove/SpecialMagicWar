using System;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class MaxHPAdditionalDataEffect : DataEffect<int>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"�ִ� ü���� �߰��ϰų� �ٿ��ּ���.";
            }
            else
            {
                return $"�ִ� ü��  {value} �߰�";
            }
        }
    }
}