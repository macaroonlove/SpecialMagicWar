using System;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class ManaRecoveryPerSecAdditionalDataEffect : DataEffect<int>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"�ʴ� ���� ȸ������ �߰��ϰų� �ٿ��ּ���.";
            }
            else if (value > 0)
            {
                return $"�ʴ� ���� ȸ����  {value} �߰�";
            }
            else
            {
                return $"�ʴ� ���� ȸ����  {Mathf.Abs(value)} ����";
            }
        }
    }
}