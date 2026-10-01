using Content.Shared._Funkystation.SISTR.Prototypes;
using Content.Shared._Funkystation.SISTRTerminal.Components;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Client._Funkystation.SISTRTerminal;

[UsedImplicitly]
public sealed partial class SistrTerminalBoundUserInterface(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    [Dependency] private SharedUserInterfaceSystem _ui = null!;
    [Dependency] private ILogManager _logManager = null!;
    [Dependency] private IPrototypeManager _protoMan = null!;
    private ISawmill? _sawmill;

    private SistrTerminalUi? _terminal;
    private List<SistrProgramPrototype> _programs = new();

    protected override void Open()
    {
        base.Open();

        _sawmill = _logManager.GetSawmill("SISTR");
        if (!EntMan.TryGetComponent<SistrTerminalComponent>(Owner, out var terminalComp))
        {
            _sawmill?.Debug($"Failed to get Terminal component for {EntMan.ToPrettyString(Owner)}.");
            return;
        }

        _terminal = this.CreateWindow<SistrTerminalUi>();
        _terminal.SistrCommandLine.RunProgram += HandleRunProgram;

        foreach (var protoId in terminalComp.Programs)
        {
            if (!_protoMan.TryIndex(protoId, out var program))
            {
                _sawmill?.Debug($"Terminal component for {EntMan.ToPrettyString(Owner)} had a non-existent program protoID {protoId}.");
                continue;
            }
            _programs.Add(program);
            _terminal.SistrCommandLine.RegisterProgram(program.Name, Loc.GetString(program.LocalizedDescription));
        }

        _terminal.SistrCommandLine.AddLineFormatted(
            FormattedMessage.FromMarkupOrThrow(
                Loc.GetString(terminalComp.StartupMessage)));
    }

    private void HandleRunProgram(string[] args)
    {
        var program = _programs.Find(p => string.Equals(p.Name, args[0], StringComparison.OrdinalIgnoreCase));
        if (program == null)
        {
            _terminal?.SistrCommandLine.AddLine($"Program {args[0]} not found");
            return;
        }
        RunProgram(program.Key);
    }

    private void RunProgram(Enum key)
    {
        var localEntity = PlayerManager.LocalEntity;
        if (localEntity == null || !EntMan.TryGetComponent<UserInterfaceComponent>(Owner, out var uiComp))
            return;

        if (!_ui.TryOpenUi((Owner, uiComp), key, localEntity.Value, true))
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
        program.ExitProgramBui += HandleExitProgram;
    }

    private void HandleExitProgram(SistrProgramBui program)
    {
        program.Close();
        _terminal?.OnProgramClosed();
    }
}

public abstract class SistrProgramBui(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    public Action<SistrProgramBui>? ExitProgramBui;
    public abstract void CreateControl(out SistrProgramControl control);

    protected virtual void OnExit()
    {
        ExitProgramBui?.Invoke(this);
    }
}
