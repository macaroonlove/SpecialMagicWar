using System;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class MoveIncreaseDataEffect : DataEffect<float>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"�̵��ӵ��� ���������� �����ּ���.";
            }
            else if (value > 0)
            {
                return $"�̵��ӵ�  {value * 100}% ����";
            }
            else
            {
                return $"�̵��ӵ�  {Mathf.Abs(value) * 100}% ����";
            }
        }
    }
}