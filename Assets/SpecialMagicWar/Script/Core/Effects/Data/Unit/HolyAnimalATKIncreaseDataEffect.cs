using System;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class HolyAnimalATKIncreaseDataEffect : DataEffect<float>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"신수 공격력을 증가·감소 시켜주세요.";
            }
            else if (value > 0)
            {
                return $"신수 공격력  {value * 100}% 증가";
            }
            else
            {
                return $"신수 공격력  {Mathf.Abs(value) * 100}% 감소";
            }
        }
    }
}