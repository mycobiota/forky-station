using Content.Client._Funkystation.UserInterface.RichText;
using Robust.Client.UserInterface.RichText;

namespace Content.Client.UserInterface.RichText;

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
