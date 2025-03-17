using System;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class AttackSpeedMultiplierDataEffect : DataEffect<float>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"���ݼӵ��� ��¡��϶� �����ּ���.";
            }
            else if (value > 0)
            {
                return $"���ݼӵ�  {value * 100}% ���";
            }
            else
            {
                return $"���ݼӵ�  {Mathf.Abs(value) * 100}% �϶�";
            }
        }
    }
}