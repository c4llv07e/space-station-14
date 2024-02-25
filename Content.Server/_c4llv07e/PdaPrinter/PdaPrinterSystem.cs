using Content.Server.Popups;
using Content.Shared._c4llv07e.PdaPrinter;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Popups;
using Content.Shared.PDA;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;

namespace Content.Server._c4llv07e.PdaPrinter;

public sealed class PdaPrinterSystem : EntitySystem
{
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly UserInterfaceSystem _ui = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly ItemSlotsSystem _slots = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<PdaPrinterComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<PdaPrinterComponent, ItemSlotInsertAttemptEvent>(OnInsertAttempt);
        SubscribeLocalEvent<PdaPrinterComponent, EntInsertedIntoContainerMessage>(OnContainerInserted);
    }

    private void OnStartup(Entity<PdaPrinterComponent> ent, ref ComponentStartup args)
    {
        _slots.AddItemSlot(ent.Owner, "pda_slot", ent.Comp.PdaSlot);
    }

    private void OnInsertAttempt(Entity<PdaPrinterComponent> ent, ref ItemSlotInsertAttemptEvent args)
    {
        if (args.Cancelled)
            return;

        if (args.User == null)
            return;

        if (!TryComp<PdaComponent>(args.Item, out var pda))
        {
            args.Cancelled = true;
            return;
        }
    }

    private void OnContainerInserted(Entity<PdaPrinterComponent> ent, ref EntInsertedIntoContainerMessage args)
    {
        var pda = ent.Comp.PdaSlot.Item;
        if (pda == null)
            return;

        if (!TryComp<PdaComponent>(pda, out var pdaComp))
            return;
        pdaComp.State = "pda-clown";
        Dirty(pda.Value, pdaComp);

        if (!TryComp<AppearanceComponent>(pda, out var appearance))
            return;
        Dirty(pda.Value, appearance);
    }
}
