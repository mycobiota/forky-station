using System.Threading.Tasks;
using Content.Server.Database._Funkystation;

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
}
