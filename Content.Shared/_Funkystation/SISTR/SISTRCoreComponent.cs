using Robust.Shared.GameStates;

namespace Content.Server._Funkystation.SistrCore;

// funky. marks the sis/tr core
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SistrCoreComponent : Component
{
    [AutoNetworkedField]
    public Queue<string> RadioMessages = new (15);
}
