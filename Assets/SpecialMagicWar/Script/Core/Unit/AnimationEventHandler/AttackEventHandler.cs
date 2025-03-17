using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    public class AttackEventHandler : MonoBehaviour
    {
        internal UnityAction onAttack;

        public void AttackEvent()
        {
            onAttack?.Invoke();
        }
    }
}