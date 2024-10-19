using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Shared.AlertLevel;

/// <summary>
/// Alert level detail. Does not contain an ID, that is handled by
/// the Levels field in AlertLevelPrototype.
/// </summary>
[Prototype]
public sealed partial class AlertLevelPrototype : IPrototype
{
    [IdDataField] public string ID { get; } = default!;

    /// <summary>
    /// In-game name of the alert.
    /// </summary>
    [DataField("name")] public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// What is announced upon this alert level change. Can be a localized string.
    /// </summary>
    [DataField("announcement")] public string Announcement { get; private set; } = string.Empty;

    /// <summary>
    /// Whether this alert level is selectable from a communications console.
    /// </summary>
    [DataField("selectable")] public bool Selectable { get; private set; } = true;

    /// <summary>
    /// If this alert level disables user selection while it is active. Beware -
    /// setting this while something is selectable will disable selection permanently!
    /// This should only apply to entities or gamemodes that auto-select an alert level,
    /// such as a nuclear bomb being set to active.
    /// </summary>
    [DataField("disableSelection")] public bool DisableSelection { get; private set; }

    /// <summary>
    /// The sound that this alert level will play in-game once selected.
    /// </summary>
    [DataField("sound")] public SoundSpecifier? Sound { get; private set; }

    /// <summary>
    /// The color that this alert level will show in-game in chat.
    /// </summary>
    [DataField("color")] public Color Color { get; private set; } = Color.White;

    /// <summary>
    /// The color to turn emergency lights on this station when they are active.
    /// </summary>
    [DataField("emergencyLightColor")] public Color EmergencyLightColor { get; private set; } = Color.FromHex("#FF4020");

    /// <summary>
    /// Will this alert level force emergency lights on for the station that's active?
    /// </summary>
    [DataField("forceEnableEmergencyLights")] public bool ForceEnableEmergencyLights { get; private set; } = false;

    /// <summary>
    /// How long it takes for the shuttle to arrive when called.
    /// </summary>
    [DataField("shuttleTime")] public TimeSpan ShuttleTime { get; private set; } = TimeSpan.FromMinutes(5);
}

