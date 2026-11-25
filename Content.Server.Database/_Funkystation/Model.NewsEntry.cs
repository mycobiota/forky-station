using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Content.Server.Database._Funkystation;

internal static class ModelNewsEntry
{
    // arbitrary number for now but we will need to decide on a max length i think
    internal const int MaxNewsEntryLength = 16384;
    internal const int EntryBriefLength = 100;
    internal const int MaxTitleLength = 60;

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NewsEntry>()
            .HasOne(n => n.CreatedBy)
            .WithMany(pl => pl.ServerNewsPublished)
            .HasForeignKey(n => n.CreatedById)
            .HasPrincipalKey(pl => pl.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

[Table("server_news_entries")]
public class NewsEntry
{
    [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, MaxLength(ModelNewsEntry.MaxTitleLength)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(ModelNewsEntry.MaxNewsEntryLength)]
    public string Message { get; set; } = string.Empty;

    [Required, MaxLength(ModelNewsEntry.EntryBriefLength)]
    public string MessageBrief { get; set; } = string.Empty;

    [Required] public DateTime CreatedAt { get; set; }

    [ForeignKey("CreatedBy")]
    public Guid? CreatedById { get; set; }
    public Player? CreatedBy { get; set; }

}
