using System.Threading.Tasks;

namespace Content.Server.Database;

public partial interface IServerDbManager
{
    Task<int> AddNewsEntry(string message, string messageBrief, DateTimeOffset createdAt);
}
