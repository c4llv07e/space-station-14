using Content.Shared.Preferences;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Client.Preferences.UI
{
    public sealed partial class HumanoidProfileEditor
    {
        private readonly IPrototypeManager _prototypeManager;
        private readonly IRobustRandom _random;

        private void RandomizeEverything()
        {
            Profile = HumanoidCharacterProfile.Random();
            UpdateControls();
            IsDirty = true;
        }

        private void RandomizeName()
        {
            if (Profile == null) return;
            var name = HumanoidCharacterProfile.GetName(Profile.Species, Profile.Gender);
            SetName(name);
            UpdateNameEdit();
        }
    }
}
