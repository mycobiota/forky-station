using Content.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;

namespace Content.Client._Funkystation.ServerNews.Eui;

public sealed class ServerNewsEditorWindow : FancyWindow
{
    public ServerNewsEditorWindow()
    {
        RobustXamlLoader.Load(this);
    }
}
