using Content.Shared.Shuttles.BUIStates;
using Content.Shared.Shuttles.Components;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client._Funkystation.SISTRTerminal.Programs.MassScanner;

[UsedImplicitly]
public sealed class SistrMassScannerBoundUserInterface(EntityUid owner, Enum uiKey) : SistrProgramBui(owner, uiKey)
{
    private SistrMassScanner? _massScanner;

    public override string Name => "massScanner";

    public override void CreateControl(out SistrProgramControl control)
    {
        _massScanner = this.CreateDisposableControl<SistrMassScanner>();
        _massScanner.ExitProgram += OnExit;
        control = _massScanner;
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);
        if (state is not NavBoundUserInterfaceState cState)
            return;

        _massScanner?.UpdateState(cState.State);
    }
}
