using System;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class MaxHPIncreaseDataEffect : DataEffect<float>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"최대 체력을 증가·감소 시켜주세요.";
            }
            else if (value > 0)
            {
                return $"최대 체력  {value * 100}% 증가";
            }
            else
            {
                return $"최대 체력  {value * 100}% 감소";
            }
        }
    }
}