using System.Threading.Tasks;
using Content.Server._Funkystation.ServerNews;

namespace Content.Server.Database;

public partial interface IServerDbManager
{
    Task<int> AddNewsEntry(string message, string messageBrief, Guid createdBy, DateTimeOffset createdAt);
    Task DeleteNewsEntry(int newsId);
    Task<List<ServerNewsRecord>> GetServerNews();
}
