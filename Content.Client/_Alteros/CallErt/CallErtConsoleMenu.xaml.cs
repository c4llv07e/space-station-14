using System.Linq;
using Content.Client.CallErt;
using Content.Client.GameTicking.Managers;
using Content.Client.Message;
using Content.Client.UserInterface.Controls;
using Content.Shared.CallErt;
using Robust.Client.AutoGenerated;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Timing;


namespace Content.Client._Alteros.CallErt
{
    [GenerateTypedNameReferences]
    public sealed partial class CallErtConsoleMenu : FancyWindow
    {
        [Dependency] private readonly IGameTiming _gameTiming = default!;
        [Dependency] private readonly ILocalizationManager _loc = default!;
        [Dependency] private readonly IEntitySystemManager _entitySystem = default!;
        private readonly ClientGameTicker _gameTicker;

        private CallErtConsoleBoundUserInterface Owner { get; set; }

        public Action<int>? RecallErt { get; set; }

        public CallErtConsoleMenu(CallErtConsoleBoundUserInterface owner)
        {
            IoCManager.InjectDependencies(this);
            _gameTicker = _entitySystem.GetEntitySystem<ClientGameTicker>();
            RobustXamlLoader.Load(this);

            Owner = owner;

            ErtGroupSelector.OnItemSelected += args =>
            {
                var metadata = ErtGroupSelector.GetItemMetadata(args.Id);
                if (metadata != null && metadata is string cast)
                {
                    Owner.ErtGroupSelected(cast);
                }
            };

            CallErt.OnPressed += (_) => Owner.CallErtButtonPressed();
            CallErt.Disabled = !owner.CanCallErt;
        }

        public void UpdateErtList(Dictionary<string, ErtGroupDetail> ertGroups, string? selectedErt)
        {
            ErtGroupSelector.Clear();

            if (ertGroups.Count == 0)
                return;

            selectedErt ??= ertGroups.First().Key;

            foreach (var (ertGroupName, ErtGroupDetail) in ertGroups)
            {
                var name = ertGroupName;
                if (Loc.TryGetString($"ert-group-name-{ErtGroupDetail.Name}", out var locName))
                {
                    name = locName;
                }
                ErtGroupSelector.AddItem(name);
                ErtGroupSelector.SetItemMetadata(ErtGroupSelector.ItemCount - 1, ertGroupName);

                if (ertGroupName == selectedErt)
                {
                    ErtGroupSelector.Select(ErtGroupSelector.ItemCount - 1);
                }
            }

            if (!ertGroups.TryGetValue(selectedErt, out var ertGroup))
                return;

            var humanListText = "";

            var humansCount = ertGroup.HumansList.Count;

            foreach (var (humanId, count) in ertGroup.HumansList)
            {
                humanListText += $"● {_loc.GetEntityData(humanId).Name}: {count}";

                if (humansCount <= 1)
                    continue;

                humanListText += "\n";
                humansCount--;
            }

            HumanListLabel.SetMarkup(humanListText);

            var waitingTime = TimeSpan.FromSeconds(ertGroup.WaitingTime);

            TimeToSpawnLabel.SetMarkup($"{waitingTime:hh':'mm':'ss}");

            var requirementsList = "";
            var requirementsCount = ertGroup.Requirements.Count;

            foreach (var (requirement, count) in ertGroup.Requirements)
            {
                var requirementsText = GetRequirementsText(requirement, count);
                requirementsList += $"● {requirementsText}";

                if (requirementsCount <= 1)
                    continue;

                requirementsList += "\n";
                requirementsCount--;
            }

            RequirementsLabel.SetMarkup(requirementsList);
        }

        private string GetRequirementsText(string requirement, int count)
        {
            return requirement switch
            {
                "RoundDuration" => $"Длительность смены дольше {count} минут.",
                "DeadHeads" => $"Погибло более {count} глав отделов.",
                "DeadPercent" => $"Погибло более {count}% членнов экипажа.",
                "ZombiePercent" => $"В зомби превращено более {count}% членов экипажа.",
                "PuddlesCount" => $"На станции более {count} луж.",
                "DangerAlarmCount" => $"На станции более {count} разгерметираций.",
                "ActiveFleshHeart" => $"На станции пробужденное сердце плоти.",
                "NukiesOnStation" => $"На станции более {count} ядерных оперативников.",
                "BlobCores" => $"На станции находятся ядра блоба.",
                _ => ""
            };
        }

        public void UpdateCalledErtList(List<CallErtGroupEnt>? calledErts)
        {
            CallErtEntriesContainer.Children.Clear();

            if (calledErts == null)
                return;

            var originalIndexes = Enumerable.Range(0, calledErts.Count).ToList();

            var sortedIndexes = originalIndexes.OrderByDescending(i => calledErts[i].CalledTime).ToList();

            foreach (var sortedIndex in sortedIndexes)
            {
                var calledErt = calledErts[sortedIndex];

                if (calledErt.ErtGroupDetail == null)
                    continue;

                var entry = new CalledErtEntry(sortedIndex, calledErt.ErtGroupDetail.Name, calledErt.CalledTime, calledErt.ArrivalTime, calledErt.Status, calledErt.Reason, recallErt: RecallErt);

                CallErtEntriesContainer.AddChild(entry);
            }
        }


        public void UpdateStationTime()
        {
            var stationTime = _gameTiming.CurTime.Subtract(_gameTicker.RoundStartTimeSpan);

            StationTimeLabel.SetMarkup(Loc.GetString(stationTime.ToString("hh\\:mm\\:ss")));
        }

        protected override void Draw(DrawingHandleScreen handle)
        {
            base.Draw(handle);

            var stationTime = _gameTiming.CurTime.Subtract(_gameTicker.RoundStartTimeSpan);

            StationTimeLabel.SetMarkup($"{stationTime:hh':'mm':'ss}");
        }
    }
}

