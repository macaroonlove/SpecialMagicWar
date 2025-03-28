using System;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class MagicPenetrationMultiplierDataEffect : DataEffect<float>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"마법 관통력을 상승·하락 시켜주세요.";
            }
            else if (value > 0)
            {
                return $"마법 관통력  {value * 100}% 상승";
            }
            else
            {
                return $"마법 관통력  {value * 100}% 하락";
            }
        }
    }
}