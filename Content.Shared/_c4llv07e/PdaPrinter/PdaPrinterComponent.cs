using Content.Shared.Containers.ItemSlots;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared._c4llv07e.PdaPrinter;

[RegisterComponent, NetworkedComponent]
public sealed partial class PdaPrinterComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public ItemSlot PdaSlot = new();
}

private Dictiorary<string, string> pdaNames =
{
    "КПК Капитана": "pda-captain",
};
