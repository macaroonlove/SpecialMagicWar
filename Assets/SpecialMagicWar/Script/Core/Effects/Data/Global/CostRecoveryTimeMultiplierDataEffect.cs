using System;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class CostRecoveryTimeMultiplierDataEffect : DataEffect<float>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"코스트 중가 속도를 상승·하락 시켜주세요.";
            }
            else if (value > 0)
            {
                return $"코스트 중가 속도  {value * 100}% 상승";
            }
            else
            {
                return $"코스트 중가 속도  {Mathf.Abs(value) * 100}% 하락";
            }
        }
    }
}