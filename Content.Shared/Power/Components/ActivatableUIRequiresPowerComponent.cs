using Robust.Shared.GameStates;

namespace Content.Shared.Power.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class ActivatableUIRequiresPowerComponent : Component
{
    /// <summary>
    /// Funky - whether this object has additional user interfaces aside from the activatable one.
    /// </summary>
    [DataField]
    public bool HasAdditionalInterfaces;
}
