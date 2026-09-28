using Robust.Shared.GameStates;

namespace Content.Shared._Funkystation.SISTRTerminal;

[RegisterComponent]
public sealed partial class SistrTerminalComponent : Component
{
    [DataField]
    public Dictionary<string, (Enum, InterfaceData)> Programs;

}
