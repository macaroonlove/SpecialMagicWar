using System;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class CostRecoveryTimeIncreaseDataEffect : DataEffect<float>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"코스트 중가 속도를 증가·감소 시켜주세요.";
            }
            else if (value > 0)
            {
                return $"코스트 중가 속도  {value * 100}% 증가";
            }
            else
            {
                return $"코스트 중가 속도  {Mathf.Abs(value) * 100}% 감소";
            }
        }
    }
}