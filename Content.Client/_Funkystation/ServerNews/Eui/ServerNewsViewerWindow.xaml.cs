using Content.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;

namespace Content.Client._Funkystation.ServerNews.Eui;

public sealed class ServerNewsViewerWindow : FancyWindow
{
    public ServerNewsViewerWindow()
    {
        RobustXamlLoader.Load(this);
    }
}
