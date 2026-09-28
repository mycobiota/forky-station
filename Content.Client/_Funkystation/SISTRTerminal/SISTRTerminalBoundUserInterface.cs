using Content.Client._Funkystation.SISTRTerminal.Programs;
using Content.Client.UserInterface.ControlExtensions;
using Content.Shared._Funkystation.SistrCore;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Shared.Utility;

namespace Content.Client._Funkystation.SISTRTerminal;

[UsedImplicitly]
public sealed partial class SistrTerminalBoundUserInterface(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    private SistrTerminalUi? _terminal;

    protected override void Open()
    {
        base.Open();

        _terminal = this.CreateWindow<SistrTerminalUi>();
    }

    public override void Update()
    {
        base.Update();

        if (_terminal?.CurrentProgram == "binarychat" && EntMan.TryGetComponent<SistrCoreComponent>(Owner, out var sistrCoreComponent))
        {
            _terminal.GetControlOfType<SistrBinaryChat>()
                .Pop()
                .Update(sistrCoreComponent.RadioMessages);
        }
    }
}
