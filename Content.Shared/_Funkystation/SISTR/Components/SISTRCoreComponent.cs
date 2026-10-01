using Content.Shared._Funkystation.SISTR;
using Robust.Shared.GameStates;

namespace Content.Shared._Funkystation.SistrCore.Components;

// funky. marks the sis/tr core
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SistrCoreComponent : Component
{
    [AutoNetworkedField, ViewVariables(VVAccess.ReadOnly)]
    public Queue<string> RadioMessages = new (SharedSistrCoreSystem.AutomaTalkHistoryLength);
}
