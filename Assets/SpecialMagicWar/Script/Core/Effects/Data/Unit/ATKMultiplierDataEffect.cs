using System;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class ATKMultiplierDataEffect : DataEffect<float>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"���ݷ��� ��¡��϶� �����ּ���.";
            }
            else if (value > 0)
            {
                return $"���ݷ�  {value * 100}% ���";
            }
            else
            {
                return $"���ݷ�  {Mathf.Abs(value) * 100}% �϶�";
            }
        }
    }
}