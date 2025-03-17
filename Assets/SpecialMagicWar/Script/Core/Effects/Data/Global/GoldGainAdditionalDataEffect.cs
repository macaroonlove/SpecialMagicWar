using System;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class GoldGainAdditionalDataEffect : DataEffect<int>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"�߰� ��� ȹ�淮�� �������ּ���.";
            }
            else if (value > 0)
            {
                return $"��� ȹ�� ��, {value} �߰� ȹ��";
            }
            else
            {
                return $"��� ȹ�� ��, {Mathf.Abs(value)} ��ŭ �����Ǿ� ȹ��";
            }
        }
    }
}