using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Shared.AlertLevel;

[Prototype]
public sealed partial class AlertLevelsPrototype : IPrototype
{
    [IdDataField] public string ID { get; } = default!;

    /// <summary>
    /// Dictionary of alert levels. Keyed by string - the string key is the most important
    /// part here. Visualizers will use this in order to dictate what alert level to show on
    /// client side sprites, and localization uses each key to dictate the alert level name.
    /// </summary>
    [DataField("levels")] public List<ProtoId<AlertLevelPrototype>> Levels = new();

    /// <summary>
    /// Default level that the station is on upon initialization.
    /// If this isn't in the dictionary, this will default to whatever .First() gives.
    /// </summary>
    [DataField("defaultLevel")] public ProtoId<AlertLevelPrototype> DefaultLevel { get; private set; } = default!;
}
