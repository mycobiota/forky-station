using Content.Client._Funkystation.SISTRTerminal.Programs;
using Content.Client.UserInterface.ControlExtensions;
using Content.Shared._Funkystation.SistrCore;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Shared.Sandboxing;
using Robust.Shared.Utility;

namespace Content.Client._Funkystation.SISTRTerminal;

[UsedImplicitly]
public sealed partial class SistrTerminalBoundUserInterface(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    [Dependency] private ISandboxHelper _sandboxHelper = null!;

    private SistrTerminalUi? _terminal;
    private Dictionary<string, Action> _programs = new();

    protected override void Open()
    {
        base.Open();

        _terminal = this.CreateWindow<SistrTerminalUi>();
        _programs.Add("automatalk", RunProgram<SistrAutomaTalkBoundUserInterface>);

        _terminal.SistrCommandLine.RunProgram += HandleRunProgram;
    }

    private void HandleRunProgram(string[] args)
    {
        if (_programs.TryGetValue(args[0], out var program))
        {
            program.Invoke();
        }
    }

    private void RunProgram<T>() where T : SistrProgramBui
    {
        if (_sandboxHelper.CreateInstance(typeof(T)) is not T program)
            return;

        var control = program.CreateSistrProgram();
        if (control == null)
        {
            program.Close();
            return;
        }

        _terminal?.OpenProgram(control);
        program.ExitProgram += HandleExitProgram;
    }

    private void HandleExitProgram<T>(T program) where T : SistrProgramBui
    {
        program.Close();
        _terminal?.OnProgramClosed();
    }
}

public abstract class SistrProgramBui(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    public Action<SistrProgramBui>? ExitProgram;
    public abstract SistrProgramControl? CreateSistrProgram();
}
