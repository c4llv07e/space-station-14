using Content.Shared.PDA;
using Content.Shared.Light;
using Robust.Client.GameObjects;
using Robust.Shared.GameStates;

namespace Content.Client.PDA;

public sealed class PdaSystem : SharedPdaSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PdaComponent, AppearanceChangeEvent>(OnAppearanceChange);
    }

    private void OnAppearanceChange(EntityUid uid, PdaComponent component, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;

        if (Appearance.TryGetData<bool>(uid, UnpoweredFlashlightVisuals.LightOn, out var isFlashlightOn, args.Component))
            args.Sprite.LayerSetVisible(PdaVisualLayers.Flashlight, isFlashlightOn);

        if (Appearance.TryGetData<bool>(uid, PdaVisuals.IdCardInserted, out var isCardInserted, args.Component))
            args.Sprite.LayerSetVisible(PdaVisualLayers.IdLight, isCardInserted);
        UpdateSprite(uid, component);
    }

    private void UpdateSprite(EntityUid uid, PdaComponent component)
    {
        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        if (component.State != null)
            sprite.LayerSetState(PdaVisualLayers.Base, component.State);

        sprite.LayerSetVisible(PdaVisualLayers.Flashlight, component.FlashlightOn);
        sprite.LayerSetVisible(PdaVisualLayers.IdLight, component.IdSlot.StartingItem != null);
    }

    protected override void OnComponentInit(EntityUid uid, PdaComponent component, ComponentInit args)
    {
        base.OnComponentInit(uid, component, args);
        UpdateSprite(uid, component);
    }

    public enum PdaVisualLayers : byte
    {
        Base,
        Flashlight,
        IdLight
    }
}
