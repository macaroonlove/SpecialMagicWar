using System;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class HolyAnimalATKIncreaseDataEffect : DataEffect<float>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"�ż� ���ݷ��� ���������� �����ּ���.";
            }
            else if (value > 0)
            {
                return $"�ż� ���ݷ�  {value * 100}% ����";
            }
            else
            {
                return $"�ż� ���ݷ�  {Mathf.Abs(value) * 100}% ����";
            }
        }
    }
}