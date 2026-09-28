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

        _terminal = this.CreateWindow<SistrTerminalUi>();
        _programs.Add("automatalk", (SistrAutomaTalkKey.Key, new InterfaceData(nameof(SistrAutomaTalkBoundUserInterface))));

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

        _ui.SetUi((Owner, uiComp), key, data);
        if (!_ui.TryOpenUi((Owner, uiComp), key, localEntity.Value))
        {
            _sawmill?.Debug("Couldn't open UI");
            return;
        }
        _ui.TryGetOpenUi((Owner, uiComp), key, out SistrProgramBui? program);
        if (program == null)
        {
            _sawmill?.Debug("Program was null");
            return;
        }

        var control = program.CreateControl();
        if (control == null)
        {
            _sawmill?.Debug("Control was null");
            program.Close();
            return;
        }

        _terminal?.OpenProgram(control);
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
    public Action<SistrProgramBui>? ExitProgram;
    public abstract SistrProgramControl? CreateControl();
}
