using System.Linq;
using System.Threading.Tasks;
using Content.Server._Funkystation.ServerNews;
using Content.Server.Database._Funkystation;
using Microsoft.EntityFrameworkCore;

namespace Content.Server.Database;

public abstract partial class ServerDbBase
{
    public virtual async Task<int> AddNewsEntry(NewsEntry newsEntry)
    {
        await using var db = await GetDb();
        db.DbContext.NewsEntry.Add(newsEntry);
        await db.DbContext.SaveChangesAsync();
        return newsEntry.Id;
    }

    public virtual async Task DeleteNewsEntry(int newsId)
    {
        await using var db = await GetDb();
        var entry = await db.DbContext.NewsEntry.SingleAsync(n => n.Id == newsId);
        db.DbContext.NewsEntry.Remove(entry);

        await db.DbContext.SaveChangesAsync();
    }

    public virtual async Task<List<ServerNewsRecord>> GetServerNews()
    {
        await using var db = await GetDb();
        return db.DbContext.NewsEntry.Include(n => n.CreatedBy)
            .Select(MakeServerNewsRecord)
            .ToList();
    }

    private ServerNewsRecord MakeServerNewsRecord(NewsEntry newsEntry)
    {
        return new ServerNewsRecord(
            Id: newsEntry.Id,
            CreatedBy: MakePlayerRecord(newsEntry.CreatedBy),
            Message: newsEntry.Message,
            MessageBrief: newsEntry.MessageBrief,
            CreatedAt: newsEntry.CreatedAt
        );
    }
}
