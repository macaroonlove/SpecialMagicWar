using System;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class WaterATKIncreaseDataEffect : DataEffect<float>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"�� Ÿ�� ���ݷ��� ���������� �����ּ���.";
            }
            else if (value > 0)
            {
                return $"�� Ÿ�� ���ݷ�  {value * 100}% ����";
            }
            else
            {
                return $"�� Ÿ�� ���ݷ�  {Mathf.Abs(value) * 100}% ����";
            }
        }
    }
}