using System.Linq;
using Content.Client.Administration.Managers;
using Content.Client.UserInterface.Systems.Chat;
using Content.IntegrationTests.Fixtures.Attributes;
using Content.IntegrationTests.Tests.Interaction;
using Content.Server.Administration.Managers;
using Content.Server.Chat.Systems;
using Content.Server.GameTicking;
using Content.Server.Power.Components;
using Content.Server.StationEvents.Events;
using Content.Shared._Funkystation.CCVar;
using Content.Shared.Administration.Managers;
using Content.Shared.Chat;
using Content.Shared.GameTicking.Components;
using Content.Shared.Power.EntitySystems;
using Robust.Client.UserInterface;
using Robust.Shared.Configuration;
using Robust.Shared.GameObjects;
using Robust.Shared.Prototypes;

namespace Content.IntegrationTests.Tests._Funkystation.Chat;

[TestFixture]
[TestOf(typeof(ChatAnnouncement)), TestOf(typeof(SharedChatSystem))]
public sealed class AnnouncementTests : InteractionTest // inherits from interaction test because im too lazy to do player setup myself
{
    [SidedDependency(Side.Server)] private ChatSystem _chatSystem = null!;
    [SidedDependency(Side.Server)] private GameTicker _ticker = null!;
    [SidedDependency(Side.Server)] private IAdminManager _adminManager = null!;
    [SidedDependency(Side.Server)] private SharedPowerReceiverSystem _powerReceiverSystem = null!;

    [SidedDependency(Side.Client)] private IUserInterfaceManager _userInterfaceManager = null!;

    private ChatUIController _playerChatController;

    private static readonly CVarDef<bool> PAEnabledCvar = PAAnnouncementCVars.PAEnabled;
    private static readonly CVarDef<bool> SistrRequiredCvar = SistrCVars.SistrRequired;
    private static readonly EntProtoId SistrCore = "SistrCore";

    private const string AnnouncementText = "Test";

    [TestPrototypes]
    private const string Prototypes = @"
- type: entity
  id: TestStationEvent
  components:
  - type: GameRule
  - type: StationEvent
    duration: 1
    startAnnouncement: Started
    endAnnouncement: Ended
  - type: TestRule
";
    private const string TestStationEvent = "TestStationEvent";

    public override async Task DoSetup()
    {
        await base.DoSetup();

        _playerChatController = _userInterfaceManager.GetUIController<ChatUIController>();
        //todo: figure out how to prevent admin alerts from showing up?
    }

    // HOW THE FUCK DO YOU WRITE THESE FUCKING THINGS???
    [Test]
    public async Task GlobalAnnouncementWithPAAndSistrDisabledTest()
    {
        // disable both cvars
        await OverrideCVar(Side.Server, PAEnabledCvar, false);
        await OverrideCVar(Side.Server, SistrRequiredCvar, false);

        var announcement = new ChatAnnouncement(AnnouncementText);

        await Client.WaitPost(() => _playerChatController.History.Clear());

        await Server.WaitPost(() =>
        {
            _chatSystem.DispatchGlobalAnnouncement(announcement);
        });

        await Pair.RunUntilSynced();

        // make sure there is only one message in the player's chat and it is the announcement
        Assert.That(_playerChatController.History, Has.Count.EqualTo(1));
        Assert.That(_playerChatController.History.Last().Msg.Message, Is.EqualTo(AnnouncementText));
    }

    [Test]
    public async Task StationEventAnnouncementSistrTest()
    {
        // disable PA, make sure SISTR is enabled
        await OverrideCVar(Side.Server, PAEnabledCvar, false);
        await OverrideCVar(Side.Server, SistrRequiredCvar, true);

        await Client.WaitPost(() => _playerChatController.History.Clear());

        await Server.WaitPost(() =>
        {
            var rule = _ticker.AddGameRule(TestStationEvent);
            Assert.That(_ticker.StartGameRule(rule));
        });

        await Pair.RunUntilSynced();

        // make sure that without SISTR, the announcement is not made
        Assert.That(_playerChatController.History, Has.Count.EqualTo(1)); // an admin alert still pops up so this is 1 todo: prevent admin alerts

        await Pair.RunSeconds(2);

        // make sure the ended announcement is not audible either
        Assert.That(_playerChatController.History, Has.Count.EqualTo(1));

        // spawn SISTR core
        var core = await Spawn(SistrCore);

        // make sure that with SISTR, announcement is made
        await Server.WaitPost(() =>
        {
            _powerReceiverSystem.SetNeedsPower(SEntMan.GetEntity(core), false);

            var rule = _ticker.AddGameRule(TestStationEvent);
            Assert.That(_ticker.StartGameRule(rule));
        });

        await Pair.RunUntilSynced();

        await Server.WaitPost(() =>
        {
            Assert.Multiple(() =>
            {
                Assert.That(TryComp<ApcPowerReceiverComponent>(core, out var comp));
                Assert.That(!comp!.NeedsPower);
                Assert.That(_powerReceiverSystem.IsPowered((SEntMan.GetEntity(core), comp)));
            });
        });

        Assert.That(_playerChatController.History, Has.Count.EqualTo(3)); // todo: why sistr not work :(
        Assert.That(_playerChatController.History.Last().Msg.Message, Is.EqualTo("Started"));

        await Pair.RunSeconds(2);

        Assert.That(_playerChatController.History, Has.Count.EqualTo(4));
        Assert.That(_playerChatController.History.Last().Msg.Message, Is.EqualTo("Ended"));
    }
}
