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
}

public class NewsEntry
{
    [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, MaxLength(ModelNewsEntry.MaxNewsEntryLength)]
    public string Message { get; set; } = string.Empty;

    [Required, MaxLength(ModelNewsEntry.EntryBriefLength)]
    public string MessageBrief { get; set; } = string.Empty;

    [Required] public DateTime CreatedAt { get; set; }
}
