using System;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class CostRecoveryTimeIncreaseDataEffect : DataEffect<float>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"�ڽ�Ʈ �߰� �ӵ��� ���������� �����ּ���.";
            }
            else if (value > 0)
            {
                return $"�ڽ�Ʈ �߰� �ӵ�  {value * 100}% ����";
            }
            else
            {
                return $"�ڽ�Ʈ �߰� �ӵ�  {Mathf.Abs(value) * 100}% ����";
            }
        }
    }
}