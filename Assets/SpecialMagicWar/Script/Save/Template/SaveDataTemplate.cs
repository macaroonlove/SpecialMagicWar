using UnityEngine;

namespace SpecialMagicWar.Save
{
    public abstract class SaveDataTemplate : ScriptableObject
    {
        public abstract void SetDefaultValues();

        public abstract bool Load(string json);

        public abstract string ToJson();

        public abstract void Clear();
    }
}