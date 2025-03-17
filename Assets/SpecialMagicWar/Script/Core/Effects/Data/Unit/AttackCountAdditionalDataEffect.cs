using System;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class AttackCountAdditionalDataEffect : DataEffect<int>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"�ִ� ���� ���� ��� ���� �߰��ϰų� �ٿ��ּ���.";
            }
            else if (value > 0)
            {
                return $"�ִ� ���� ���� ��� ��  {value} �߰�";
            }
            else
            {
                return $"�ִ� ���� ���� ��� ��  {Mathf.Abs(value)} ����";
            }
        }
    }
}