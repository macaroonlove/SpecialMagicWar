using System;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class CostRecoveryTimeAdditionalDataEffect : DataEffect<int>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"�ڽ�Ʈ �߰� �ӵ��� �߰��ϰų� �ٿ��ּ���.";
            }
            else if (value > 0)
            {
                return $"�ڽ�Ʈ �߰� �ӵ�  {value} ����";
            }
            else
            {
                return $"�ڽ�Ʈ �߰� �ӵ�  {Mathf.Abs(value)} �߰�";
            }
        }
    }
}