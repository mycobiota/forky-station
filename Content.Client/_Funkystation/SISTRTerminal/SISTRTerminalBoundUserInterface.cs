using Content.Client._Funkystation.Unions.UI;
using Robust.Client.UserInterface;
using Robust.Shared.Utility;

namespace Content.Client._Funkystation.SISTRTerminal;

public sealed partial class SistrTerminalBoundUserInterface(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    private SistrTerminalUi? _terminal;

    private Dictionary<string, Action<string[]>> _commands = new ();

    protected override void Open()
    {
        base.Open();

        // todo: better way of defining commands
        _commands.Add("clear", ClearTerminal);
        _commands.Add("echo", EchoCommand);

        _terminal = this.CreateWindow<SistrTerminalUi>();
        _terminal.CommandEntered += OnCommandEntered;
    }

    private void OnCommandEntered(string input)
    {
        ParseCommand(input);
    }

    private void ParseCommand(string command)
    {
        var arguments = command.Split(' ');
        _commands.TryGetValue(arguments[0], out var action);
        action?.Invoke(arguments[1..]);
    }

    private void ClearTerminal(string[] args)
    {
        _terminal?.ClearTerminal();
    }

    private void EchoCommand(string[] args)
    {
        var input = string.Join(' ', args);
        _terminal?.AddLine(input);
    }
}
