using System;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class CriticalHitChanceAdditionalDataEffect : DataEffect<int>
    {
        public override string GetDescription()
        {
            if (value == 0)
            {
                return $"치명타 확률을 추가하거나 줄여주세요.";
            }
            else
            {
                return $"치명타 확률  {value} 추가";
            }
        }
    }
}