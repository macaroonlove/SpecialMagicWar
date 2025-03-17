using System;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class GoldGainIncreaseDataEffect : DataEffect<float>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"��� ȹ�淮�� ���������� �����ּ���.";
            }
            else if (value > 0)
            {
                return $"��� ȹ�淮  {value * 100}% ����";
            }
            else
            {
                return $"��� ȹ�淮  {Mathf.Abs(value) * 100}% ����";
            }
        }
    }
}