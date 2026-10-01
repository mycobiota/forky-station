using System.Numerics;
using Content.Client.Resources;
using Content.Client.Stylesheets;
using Content.Client.Stylesheets.Stylesheets;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Utility;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client._Funkystation.SISTRTerminal.Sheetlets;

[CommonSheetlet]
public sealed class SistrTerminalSheetlet : Sheetlet<NanotrasenStylesheet>
{
    public override StyleRule[] GetRules(NanotrasenStylesheet sheet, object config)
    {
        var robotoMono11 = ResCache.GetFont("/Fonts/RobotoMono/RobotoMono-Regular.ttf", size: 11);

        var internalPanel = new StyleBoxFlat
        {
            BackgroundColor = Color.FromHex("#486859"),
            BorderThickness = new Thickness(5),
            BorderColor = Color.FromHex("#2c3b39"),
        };
        var borderPanel = new StyleBoxFlat
        {
            BackgroundColor = Color.FromHex("#886e6a"),
        };

        return
        [
            E<PanelContainer>()
                .Identifier("SistrTerminalPanel")
                .Panel(internalPanel),
            E<PanelContainer>()
                .Identifier("SistrMarginBorder")
                .Panel(borderPanel),
            E<Label>()
                .Identifier("SistrMarginLabel")
                .Font(sheet.BaseFont.GetFont(8))
                .FontColor(Color.FromHex("#2b2322")),
            E<OutputPanel>()
                .Identifier("CommandLineOutput")
                .Prop(Label.StylePropertyFont, robotoMono11),
        ];
    }
}
