using Content.Client._Funkystation.UserInterface.RichText;
using Robust.Client.UserInterface.RichText;

namespace Content.Client.UserInterface.RichText;

/// <summary>
/// <see cref="Robust.Client.UserInterface.RichTextEntry.DefaultTags"/> is inside an internal access class that lives elsewhere :(
/// so we need to make a copy of it to do anything with it
/// </summary>
public static class RichTextTags
{
    /// Mirrored from <see cref="Robust.Client.UserInterface.RichTextEntry"/>
    public static readonly Type[] DefaultTags =
    [
        typeof(BoldItalicTag),
        typeof(BoldTag),
        typeof(BulletTag),
        typeof(ColorTag),
        typeof(HeadingTag),
        typeof(ItalicTag)
    ];

    public static readonly Type[] DefaultWithRadioChannel =
    [
        typeof(BoldItalicTag),
        typeof(BoldTag),
        typeof(BulletTag),
        typeof(ColorTag),
        typeof(HeadingTag),
        typeof(ItalicTag),
        typeof(RadioChannelColorTag)
    ];
}
