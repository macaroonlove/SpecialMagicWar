using System;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class AvoidanceAdditionalDataEffect : DataEffect<int>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"ȸ�� Ȯ���� �߰��ϰų� �ٿ��ּ���.";
            }
            else if (value > 0)
            {
                return $"ȸ�� Ȯ��  {value} �߰�";
            }
            else
            {
                return $"ȸ�� Ȯ��  {Mathf.Abs(value)} ����";
            }
        }
    }
}