using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Content.Server.Database._Funkystation;

internal static class ModelNewsEntry
{
    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NewsEntry>();
    }
}

public class NewsEntry
{
    [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] public string Message { get; set; } = string.Empty;

    [ForeignKey("CreatedBy")] public Guid? CreatedById { get; set; }
    public Player? CreatedBy { get; set; }

    [Required] public DateTime CreatedAt { get; set; }

    [ForeignKey("LastEditedBy")] public Guid? LastEditedById { get; set; }
    public Player? LastEditedBy { get; set; }

    public DateTime? LastEditedAt { get; set; }

    public bool Deleted { get; set; }
    [ForeignKey("DeletedBy")] public Guid? DeletedById { get; set; }
    public Player? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
}
