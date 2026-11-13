using System.Threading.Tasks;
using Content.Server.Database._Funkystation;

namespace Content.Server.Database;

public sealed partial class ServerDbManager
{
    public Task<int> AddNewsEntry(string message, string messageBrief, DateTimeOffset createdAt)
    {
        DbWriteOpsMetric.Inc();
        var news = new NewsEntry
        {
            Message = message,
            MessageBrief = messageBrief,
            CreatedAt = createdAt.UtcDateTime,
        };

        return RunDbCommand(() => _db.AddNewsEntry(news));
    }
}
