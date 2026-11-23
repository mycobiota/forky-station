using System.Threading.Tasks;
using Content.Server.Administration.Managers;
using Content.Server.Administration.Notes;
using Content.Server.Chat.Managers;
using Content.Server.Database;
using Content.Server.EUI;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.Player;

namespace Content.Server._Funkystation.ServerNews;

public sealed partial class ServerNewsManager : IServerNewsManager, IPostInjectInit
{
    [Dependency] private IAdminManager _admins = null!;
    [Dependency] private IServerDbManager _db = null!;
    [Dependency] private ILogManager _logManager = null!;
    [Dependency] private EuiManager _euis = null!;
    [Dependency] private IEntitySystemManager _systems = null!;
    [Dependency] private IConfigurationManager _config = null!;
    [Dependency] private IPlayerManager _player = null!;
    [Dependency] private ILocalizationManager _loc = null!;
    [Dependency] private IChatManager _chat = null!;

    private ISawmill _sawmill = null!;
    private const string SawmillId = "server_news_manager";

    public event Action<int>? NewsPublished;
    public event Action? NewsEdited;
    public event Action? NewsDeleted;

    public bool CanPublish(ICommonSession? admin)
    {
        return admin != null && _admins.HasAdminFlag(admin, AdminFlags.ServerNews);
    }

    public Task OpenViewerEui(ICommonSession player)
    {
        throw new NotImplementedException();
    }

    public Task OpenEditorEui(ICommonSession admin)
    {
        throw new NotImplementedException();
    }

    public async Task<int?> PublishServerNews(ICommonSession author, string message)
    {
        if (!CanPublish(author))
        {
            _sawmill.Warning($"Author {author.Name} tried to publish server news without the required permission to.");
            return null;
        }

        var messageBrief = message.Length > 100 ? message[..100] : message;
        var newsId = await _db.AddNewsEntry(message, messageBrief, DateTime.UtcNow);
        NewsPublished?.Invoke(newsId);
        return newsId;
    }

    public Task EditServerNews(int newsId, ICommonSession editedBy, string message)
    {
        throw new NotImplementedException();
    }

    public Task DeleteServerNews(int newsId, ICommonSession deletedBy)
    {
        throw new NotImplementedException();
    }

    // todo: get only the most recent no. of entries / a certain range of entries (pages and stuff)
    public async Task<List<ServerNewsRecord>> GetServerNews()
    {
        return await _db.GetServerNews();
    }

    public void PostInject()
    {
        _sawmill = _logManager.GetSawmill(SawmillId);
    }
}
