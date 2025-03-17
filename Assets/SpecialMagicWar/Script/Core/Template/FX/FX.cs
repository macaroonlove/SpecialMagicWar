using UnityEngine;

namespace SpecialMagicWar.Core
{
    public abstract class FX : ScriptableObject
    {
        public abstract void Play(Unit target);

        public abstract void Play(Vector3 pos);
    }
}