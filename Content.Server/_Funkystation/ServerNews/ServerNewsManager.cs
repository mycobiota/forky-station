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

public sealed partial class ServerNewsManager : IServerNewsManager
{
    [Dependency] private IAdminManager _admins = default!;
    [Dependency] private IServerDbManager _db = default!;
    [Dependency] private ILogManager _logManager = default!;
    [Dependency] private EuiManager _euis = default!;
    [Dependency] private IEntitySystemManager _systems = default!;
    [Dependency] private IConfigurationManager _config = default!;
    [Dependency] private IPlayerManager _player = default!;
    [Dependency] private ILocalizationManager _loc = default!;
    [Dependency] private IChatManager _chat = default!;

    public event Action<int>? NewsPublished;
    public event Action? NewsEdited;
    public event Action? NewsDeleted;

    public bool CanCreate(ICommonSession admin)
    {
        return CanEdit(admin);
    }

    public bool CanDelete(ICommonSession admin)
    {
        return CanEdit(admin);
    }

    public bool CanEdit(ICommonSession admin)
    {
        return _admins.HasAdminFlag(admin, AdminFlags.ServerNews);
    }

    public Task OpenEui(ICommonSession player)
    {
        throw new NotImplementedException();
    }

    public Task OpenEditorEui(ICommonSession admin)
    {
        throw new NotImplementedException();
    }

    public async Task<int> PublishServerNews(ICommonSession author, string message)
    {
        var newsId = await _db.AddNewsEntry(message, message[..100], DateTime.UtcNow);
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
}
