using Robust.Shared.GameStates;

namespace Content.Shared._Funkystation.SistrCore;

// funky. marks the sis/tr core
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class SistrCoreComponent : Component
{
    [AutoNetworkedField, ViewVariables(VVAccess.ReadOnly)]
    public Queue<string> RadioMessages = new (15);
}
