using System.Linq;
using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Toolshed;

namespace Content.Server._Funkystation.ServerNews;

[ToolshedCommand(Name = "publish_news"), AdminCommand(AdminFlags.ServerNews)]
public sealed partial class PublishNewsCommand : ToolshedCommand
{
    [Dependency] private IServerNewsManager _serverNewsManager = null!;

    // placeholder until ui is implemented
    [CommandImplementation]
    public void PublishNews(IInvocationContext ctx, string text)
    {
        var author = ctx.Session;
        if (!_serverNewsManager.CanPublish(author))
            return;

        _serverNewsManager.PublishServerNews(author!, text);
    }
}

[ToolshedCommand(Name = "delete_news"), AdminCommand(AdminFlags.ServerNews)]
public sealed partial class DeleteNewsCommand : ToolshedCommand
{
    [Dependency] private IServerNewsManager _serverNewsManager = null!;

    [CommandImplementation]
    public async void DeleteNews(IInvocationContext ctx, int id)
    {
        var author = ctx.Session;
        if (!_serverNewsManager.CanPublish(author))
            return;

        var news = await _serverNewsManager.GetServerNews();
        if (news.Exists(n => n.Id == id))
            await _serverNewsManager.DeleteServerNews(id, ctx.Session!);
    }
}

[ToolshedCommand(Name = "get_news"), AnyCommand]
public sealed partial class GetNewsCommand : ToolshedCommand
{
    [Dependency] private IServerNewsManager _serverNewsManager = null!;

    [CommandImplementation]
    public async void GetNews(IInvocationContext ctx)
    {
        var news = await _serverNewsManager.GetServerNews();

        foreach (var entry in news)
        {
            if (entry.CreatedBy is null)
                ctx.WriteLine("no author for this entry");
            ctx.WriteLine($"entry id: {entry.Id}, author: {entry.CreatedBy} brief: {entry.MessageBrief}, datetime: {entry.CreatedAt}");
        }
    }
}
