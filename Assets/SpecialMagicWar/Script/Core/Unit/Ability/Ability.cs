using UnityEngine;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// ��� �ɷµ��� �߻� Ŭ����
    /// </summary>
    public abstract class Ability : MonoBehaviour
    {
        internal Unit unit { get; private set; }

        internal virtual void Initialize(Unit unit)
        {
            this.unit = unit;
        }

        internal virtual void Deinitialize()
        {

        }

        internal virtual void UpdateAbility()
        {

        }
    }
}