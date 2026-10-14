// Funky - Carpmosia-rework - Better flavor text
using Content.Shared.Examine;
using Content.Shared.IdentityManagement;
using Robust.Shared.Utility;

namespace Content.Shared.DetailExaminable;

public sealed partial class DetailExaminableSystem : EntitySystem
{
    // Funky - carpmosia edit - bake flavor text into the examine popup rather than using a separate button for it
    [SubscribeLocalEvent]
    private void OnExamined(Entity<DetailExaminableComponent> ent, ref ExaminedEvent args)
    {
        if (Identity.Name(args.Examined, EntityManager) != MetaData(args.Examined).EntityName)
            return;

        if (!args.IsInDetailsRange)
            return;

        var message = FormattedMessage.FromMarkupPermissive($"[italic][color=#c8c8c8]{ent.Comp.Content}[/color][/italic]");
        args.PushMessage(message, priority: -10);
    }
}
