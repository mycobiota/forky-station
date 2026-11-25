using System.Threading.Tasks;
using Content.Server._Funkystation.ServerNews;
using Content.Server.Database._Funkystation;

namespace Content.Server.Database;

public sealed partial class ServerDbManager
{
    public Task<int> AddNewsEntry(string message, string messageBrief, Guid createdBy, DateTimeOffset createdAt)
    {
        DbWriteOpsMetric.Inc();
        var news = new NewsEntry
        {
            Message = message,
            MessageBrief = messageBrief,
            CreatedById = createdBy,
            CreatedAt = createdAt.UtcDateTime,
        };

        return RunDbCommand(() => _db.AddNewsEntry(news));
    }

    public Task<List<ServerNewsRecord>> GetServerNews()
    {
        DbReadOpsMetric.Inc();
        return RunDbCommand(() => _db.GetServerNews());
    }
}
