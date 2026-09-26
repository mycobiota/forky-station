using Robust.Shared.GameStates;

namespace Content.Shared._Funkystation.SISTRTerminal;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SistrTerminalComponent : Component
{
    [DataField, AutoNetworkedField]
    public List<string> Messages = [];
}
