using Content.Client.Administration.Managers;
using Content.Shared.CCVar;
using Robust.Client.UserInterface;
using Robust.Shared.Configuration;


namespace Content.Client.DebugMon;

/// <summary>
/// This handles preventing certain debug monitors from appearing.
/// </summary>
public sealed class DebugMonitorSystem : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IClientAdminManager _admin = default!;
    [Dependency] private readonly IUserInterfaceManager _userInterface = default!;

    public DebugMonitorSystem()
    {
        IoCManager.InjectDependencies(this);
        _cfg.OnValueChanged(CCVars.DebugCoordinatesAdminOnly, OnConfigChaged, true);
        _admin.AdminStatusUpdated += OnAdminEvent;
    }

    private void OnAdminEvent()
    {
        _userInterface.DebugMonitors.SetMonitor(
            DebugMonitor.Coords,
            !_cfg.GetCVar(CCVars.DebugCoordinatesAdminOnly) || _admin.IsActive());
    }

    private void OnConfigChaged(bool value)
    {
        _userInterface.DebugMonitors.SetMonitor(DebugMonitor.Coords, !value || _admin.IsActive());
    }
}
