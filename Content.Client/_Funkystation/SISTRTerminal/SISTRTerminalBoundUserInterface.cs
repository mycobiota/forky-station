using Content.Client._Funkystation.SISTRTerminal.Programs.AutomaTalk;
using Content.Shared._Funkystation.SISTRTerminal;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client._Funkystation.SISTRTerminal;

[UsedImplicitly]
public sealed partial class SistrTerminalBoundUserInterface(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    [Dependency] private SharedUserInterfaceSystem _ui = null!;
    [Dependency] private ILogManager _logManager = null!;
    private ISawmill? _sawmill;

    private SistrTerminalUi? _terminal;
    private Dictionary<string, (Enum, InterfaceData)> _programs = new();

    protected override void Open()
    {
        base.Open();

        _sawmill = _logManager.GetSawmill("SISTR");
        if (!EntMan.TryGetComponent<SistrTerminalComponent>(Owner, out var terminalComp))
        {
            _sawmill?.Debug($"Failed to get Terminal component for {EntMan.ToPrettyString(Owner)}.");
            return;
        }

        _programs = terminalComp.Programs;

        _terminal = this.CreateWindow<SistrTerminalUi>();

        _terminal.SistrCommandLine.RunProgram += HandleRunProgram;

    }

    private void HandleRunProgram(string[] args)
    {
        if (_programs.TryGetValue(args[0], out var p))
        {
            var (key, data) = p;
            RunProgram(key, data);
        }
        else
        {
            _terminal?.SistrCommandLine.AddLine($"Program {args[0]} not found");
        }
    }

    private void RunProgram(Enum key, InterfaceData data)
    {
        var localEntity = PlayerManager.LocalEntity;
        if (localEntity == null || !EntMan.TryGetComponent<UserInterfaceComponent>(Owner, out var uiComp))
            return;

        if (!_ui.TryOpenUi((Owner, uiComp), key, localEntity.Value, true)) // the issue is that youre trying to do this without telling the server
        {
            _sawmill?.Debug("Couldn't open UI");
            return;
        }
        if (!_ui.TryGetOpenUi((Owner, uiComp), key, out SistrProgramBui? program))
        {
            _sawmill?.Debug("Program was null");
            return;
        }

        program.CreateControl(out var control);
        _terminal?.OpenProgram(ref control);
        program.ExitProgram += HandleExitProgram;
    }

    private void HandleExitProgram(SistrProgramBui program)
    {
        program.Close();
        _terminal?.OnProgramClosed();
    }
}

public abstract class SistrProgramBui(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    public abstract string Name { get; }
    public Action<SistrProgramBui>? ExitProgram;
    public abstract void CreateControl(out SistrProgramControl control);

    protected virtual void OnExit()
    {
        ExitProgram?.Invoke(this);
    }
}
