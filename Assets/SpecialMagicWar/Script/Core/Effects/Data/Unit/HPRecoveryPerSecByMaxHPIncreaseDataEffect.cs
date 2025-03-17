using System;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class HPRecoveryPerSecByMaxHPIncreaseDataEffect : DataEffect<float>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"�ִ� ü�� ��� �ʴ� ü�� ȸ������ ���������� �����ּ���.";
            }
            else if (value > 0)
            {
                return $"�ִ� ü�� ��� �ʴ� ü�� ȸ����  {value * 100}% ����";
            }
            else
            {
                return $"�ִ� ü�� ��� �ʴ� ü�� ȸ����  {value * 100}% ����";
            }
        }
    }
}