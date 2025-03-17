using System;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class ManaRecoveryPerSecIncreaseDataEffect : DataEffect<float>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"�ʴ� ���� ȸ������ ���������� �����ּ���.";
            }
            else if (value > 0)
            {
                return $"�ʴ� ���� ȸ����  {value * 100}% ����";
            }
            else
            {
                return $"�ʴ� ���� ȸ����  {value * 100}% ����";
            }
        }
    }
}