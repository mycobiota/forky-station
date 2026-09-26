using Content.Client._Funkystation.Unions.UI;
using Robust.Client.UserInterface;

namespace Content.Client._Funkystation.SISTRTerminal;

public sealed partial class SistrTerminalBoundUserInterface(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    private SistrTerminalUi? _terminal;
    protected override void Open()
    {
        base.Open();

        _terminal = this.CreateWindow<SistrTerminalUi>();
        _terminal.CommandEntered += s =>
        {
            _terminal.AddLine(s);
        };
    }
}
