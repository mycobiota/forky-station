using System.Threading.Tasks;
using Robust.Shared.Player;

namespace Content.Server._Funkystation.ServerNews;

// very much copying the homework of IAdminNotesManager
// and speculating about what sort of things you should be able to do
// specifically "ServerNews" to distinguish from the news published by reporters in-game
public interface IServerNewsManager
{
    event Action? NewsPublished;
    event Action? NewsEdited;
    event Action? NewsDeleted;

    bool CanCreate(ICommonSession admin);
    bool CanDelete(ICommonSession admin);
    bool CanEdit(ICommonSession admin);

    Task OpenEui(ICommonSession player);
    Task OpenEditorEui(ICommonSession admin);
    Task PublishServerNews(ICommonSession author, string message);
    Task DeleteServerNews(int newsId, ICommonSession deletedBy);
    Task EditServerNews(int newsId, ICommonSession editedBy, string message);
}
