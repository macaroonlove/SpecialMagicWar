using System;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class ATKAdditionalDataEffect : DataEffect<int>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"���ݷ��� �߰��ϰų� �ٿ��ּ���.";
            }
            else if (value > 0)
            {
                return $"���ݷ�  {value} �߰�";
            }
            else
            {
                return $"���ݷ�  {Mathf.Abs(value)} ����";
            }
        }
    }
}