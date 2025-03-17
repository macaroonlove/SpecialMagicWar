using ScriptableObjectArchitecture;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [CreateAssetMenu(fileName = "UnitEvent.asset", menuName = "Templates/Event/Unit Event", order = 1)]
    public sealed class UnitEvent : GameEventBase<Unit, Unit>
    {

    }
}