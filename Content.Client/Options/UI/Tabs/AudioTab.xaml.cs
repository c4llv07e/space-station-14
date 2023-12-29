using Content.Client.Audio;
using Content.Shared.CCVar;
using Content.Shared.Corvax.CCCVars;
using Robust.Client.Audio;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared;
using Robust.Shared.Configuration;
using Range = Robust.Client.UserInterface.Controls.Range;

namespace Content.Client.Options.UI.Tabs
{
    [GenerateTypedNameReferences]
    public sealed partial class AudioTab : Control
    {
        [Dependency] private readonly IConfigurationManager _cfg = default!;
        private readonly IAudioManager _audio;

        public AudioTab()
        {
            RobustXamlLoader.Load(this);
            IoCManager.InjectDependencies(this);

            _audio = IoCManager.Resolve<IAudioManager>();
            LobbyMusicCheckBox.Pressed = _cfg.GetCVar(CCVars.LobbyMusicEnabled);
            RestartSoundsCheckBox.Pressed = _cfg.GetCVar(CCVars.RestartSoundsEnabled);
            EventMusicCheckBox.Pressed = _cfg.GetCVar(CCVars.EventMusicEnabled);
            AdminSoundsCheckBox.Pressed = _cfg.GetCVar(CCVars.AdminSoundsEnabled);
            AHelpSoundsCheckBox.Pressed = _cfg.GetCVar(CCVars.AHelpSoundsEnabled);

            ApplyButton.OnPressed += OnApplyButtonPressed;
            ResetButton.OnPressed += OnResetButtonPressed;
            MasterVolumeSlider.OnValueChanged += OnMasterVolumeSliderChanged;
            MidiVolumeSlider.OnValueChanged += OnMidiVolumeSliderChanged;
            AmbientMusicVolumeSlider.OnValueChanged += OnAmbientMusicVolumeSliderChanged;
            AmbienceVolumeSlider.OnValueChanged += OnAmbienceVolumeSliderChanged;
            AHelpVolumeSlider.OnValueChanged += OnAHelpVolumeSliderChanged;
            AmbienceSoundsSlider.OnValueChanged += OnAmbienceSoundsSliderChanged;
            LobbyVolumeSlider.OnValueChanged += OnLobbyVolumeSliderChanged;
            InterfaceVolumeSlider.OnValueChanged += OnInterfaceVolumeSliderChanged;
            TtsVolumeSlider.OnValueChanged += OnTtsVolumeSliderChanged; // Corvax-TTS
            TtsAnnounceVolumeSlider.OnValueChanged += OnTtsAnnounceVolumeSliderChanged;
            LobbyMusicCheckBox.OnToggled += OnLobbyMusicCheckToggled;
            RestartSoundsCheckBox.OnToggled += OnRestartSoundsCheckToggled;
            EventMusicCheckBox.OnToggled += OnEventMusicCheckToggled;
            AdminSoundsCheckBox.OnToggled += OnAdminSoundsCheckToggled;
            AHelpSoundsCheckBox.OnToggled += OnAHelpSoundsCheckToggled;

            AmbienceSoundsSlider.MinValue = _cfg.GetCVar(CCVars.MinMaxAmbientSourcesConfigured);
            AmbienceSoundsSlider.MaxValue = _cfg.GetCVar(CCVars.MaxMaxAmbientSourcesConfigured);

            Reset();
        }

        protected override void Dispose(bool disposing)
        {
            ApplyButton.OnPressed -= OnApplyButtonPressed;
            ResetButton.OnPressed -= OnResetButtonPressed;
            MasterVolumeSlider.OnValueChanged -= OnMasterVolumeSliderChanged;
            MidiVolumeSlider.OnValueChanged -= OnMidiVolumeSliderChanged;
            AmbientMusicVolumeSlider.OnValueChanged -= OnAmbientMusicVolumeSliderChanged;
            AmbienceVolumeSlider.OnValueChanged -= OnAmbienceVolumeSliderChanged;
            AHelpVolumeSlider.OnValueChanged -= OnAHelpVolumeSliderChanged;
            LobbyVolumeSlider.OnValueChanged -= OnLobbyVolumeSliderChanged;
            InterfaceVolumeSlider.OnValueChanged -= OnInterfaceVolumeSliderChanged;
            TtsVolumeSlider.OnValueChanged -= OnTtsVolumeSliderChanged; // Corvax-TTS
            TtsAnnounceVolumeSlider.OnValueChanged -= OnTtsAnnounceVolumeSliderChanged;
            base.Dispose(disposing);
        }

        private void OnLobbyVolumeSliderChanged(Range obj)
        {
            UpdateChanges();
        }

        private void OnInterfaceVolumeSliderChanged(Range obj)
        {
            UpdateChanges();
        }

        private void OnAmbientMusicVolumeSliderChanged(Range obj)
        {
            UpdateChanges();
        }

        private void OnAmbienceVolumeSliderChanged(Range obj)
        {
            UpdateChanges();
        }

        private void OnAHelpVolumeSliderChanged(Range obj)
        {
            UpdateChanges();
        }

        private void OnAmbienceSoundsSliderChanged(Range obj)
        {
            UpdateChanges();
        }

        private void OnMasterVolumeSliderChanged(Range range)
        {
            _audio.SetMasterGain(MasterVolumeSlider.Value / 100f * ContentAudioSystem.MasterVolumeMultiplier);
            UpdateChanges();
        }

        private void OnMidiVolumeSliderChanged(Range range)
        {
            UpdateChanges();
        }

        // Corvax-TTS-Start
        private void OnTtsVolumeSliderChanged(Range obj)
        {
            UpdateChanges();
        }
        // Corvax-TTS-End

        private void OnTtsAnnounceVolumeSliderChanged(Range obj)
        {
            UpdateChanges();
        }

        private void OnTtsRadioVolumeSliderChanged(Range obj)
        {
            UpdateChanges();
        }

        private void OnLobbyMusicCheckToggled(BaseButton.ButtonEventArgs args)
        {
            UpdateChanges();
        }
        private void OnRestartSoundsCheckToggled(BaseButton.ButtonEventArgs args)
        {
            UpdateChanges();
        }
        private void OnAHelpSoundsCheckToggled(BaseButton.ButtonEventArgs args)
        {
            UpdateChanges();
        }

        private void OnEventMusicCheckToggled(BaseButton.ButtonEventArgs args)
        {
            UpdateChanges();
        }

        private void OnAdminSoundsCheckToggled(BaseButton.ButtonEventArgs args)
        {
            UpdateChanges();
        }

        private void OnApplyButtonPressed(BaseButton.ButtonEventArgs args)
        {
            _cfg.SetCVar(CVars.AudioMasterVolume, MasterVolumeSlider.Value / 100f * ContentAudioSystem.MasterVolumeMultiplier);
            // Want the CVar updated values to have the multiplier applied
            // For the UI we just display 0-100 still elsewhere
            _cfg.SetCVar(CVars.MidiVolume, MidiVolumeSlider.Value / 100f * ContentAudioSystem.MidiVolumeMultiplier);
            _cfg.SetCVar(CCVars.AmbienceVolume, AmbienceVolumeSlider.Value / 100f * ContentAudioSystem.AmbienceMultiplier);
            _cfg.SetCVar(CCVars.AmbientMusicVolume, AmbientMusicVolumeSlider.Value / 100f * ContentAudioSystem.AmbientMusicMultiplier);
            _cfg.SetCVar(CCVars.LobbyMusicVolume, LobbyVolumeSlider.Value / 100f * ContentAudioSystem.LobbyMultiplier);
            _cfg.SetCVar(CCVars.InterfaceVolume, InterfaceVolumeSlider.Value / 100f * ContentAudioSystem.InterfaceMultiplier);
            _cfg.SetCVar(CCCVars.TTSVolume, TtsVolumeSlider.Value / 100f * ContentAudioSystem.TtsMultiplier); // Corvax-TTS
            _cfg.SetCVar(CCCVars.TTSRadioVolume, TtsRadioVolumeSlider.Value / 100f * ContentAudioSystem.TtsMultiplier);
            _cfg.SetCVar(CCCVars.TTSAnnounceVolume, TtsAnnounceVolumeSlider.Value / 100f * ContentAudioSystem.TtsMultiplier);

            _cfg.SetCVar(CCVars.MaxAmbientSources, (int)AmbienceSoundsSlider.Value);

            _cfg.SetCVar(CCVars.LobbyMusicEnabled, LobbyMusicCheckBox.Pressed);
            _cfg.SetCVar(CCVars.RestartSoundsEnabled, RestartSoundsCheckBox.Pressed);
            _cfg.SetCVar(CCVars.EventMusicEnabled, EventMusicCheckBox.Pressed);
            _cfg.SetCVar(CCVars.AdminSoundsEnabled, AdminSoundsCheckBox.Pressed);
            _cfg.SetCVar(CCVars.AHelpSoundsEnabled, AHelpSoundsCheckBox.Pressed);
            _cfg.SaveToFile();
            UpdateChanges();
        }

        private void OnResetButtonPressed(BaseButton.ButtonEventArgs args)
        {
            Reset();
        }

        private void Reset()
        {
            MasterVolumeSlider.Value = _cfg.GetCVar(CVars.AudioMasterVolume) * 100f / ContentAudioSystem.MasterVolumeMultiplier;
            MidiVolumeSlider.Value = _cfg.GetCVar(CVars.MidiVolume) * 100f / ContentAudioSystem.MidiVolumeMultiplier;
            AmbienceVolumeSlider.Value = _cfg.GetCVar(CCVars.AmbienceVolume) * 100f / ContentAudioSystem.AmbienceMultiplier;
            AmbientMusicVolumeSlider.Value = _cfg.GetCVar(CCVars.AmbientMusicVolume) * 100f / ContentAudioSystem.AmbientMusicMultiplier;
            LobbyVolumeSlider.Value = _cfg.GetCVar(CCVars.LobbyMusicVolume) * 100f / ContentAudioSystem.LobbyMultiplier;
            InterfaceVolumeSlider.Value = _cfg.GetCVar(CCVars.InterfaceVolume) * 100f / ContentAudioSystem.InterfaceMultiplier;
            TtsVolumeSlider.Value = _cfg.GetCVar(CCCVars.TTSVolume) * 100f / ContentAudioSystem.TtsMultiplier; // Corvax-TTS
            TtsRadioVolumeSlider.Value = _cfg.GetCVar(CCCVars.TTSRadioVolume) * 100f / ContentAudioSystem.TtsMultiplier;
            TtsAnnounceVolumeSlider.Value = _cfg.GetCVar(CCCVars.TTSAnnounceVolume) * 100f / ContentAudioSystem.TtsMultiplier;

            AmbienceSoundsSlider.Value = _cfg.GetCVar(CCVars.MaxAmbientSources);

            LobbyMusicCheckBox.Pressed = _cfg.GetCVar(CCVars.LobbyMusicEnabled);
            RestartSoundsCheckBox.Pressed = _cfg.GetCVar(CCVars.RestartSoundsEnabled);
            EventMusicCheckBox.Pressed = _cfg.GetCVar(CCVars.EventMusicEnabled);
            AdminSoundsCheckBox.Pressed = _cfg.GetCVar(CCVars.AdminSoundsEnabled);
            AHelpSoundsCheckBox.Pressed = _cfg.GetCVar(CCVars.AHelpSoundsEnabled);
            UpdateChanges();
        }

        private void UpdateChanges()
        {
            // y'all need jesus.
            var isMasterVolumeSame =
                Math.Abs(MasterVolumeSlider.Value - _cfg.GetCVar(CVars.AudioMasterVolume) * 100f / ContentAudioSystem.MasterVolumeMultiplier) < 0.01f;
            var isMidiVolumeSame =
                Math.Abs(MidiVolumeSlider.Value - _cfg.GetCVar(CVars.MidiVolume) * 100f / ContentAudioSystem.MidiVolumeMultiplier) < 0.01f;
            var isAmbientVolumeSame =
                Math.Abs(AmbienceVolumeSlider.Value - _cfg.GetCVar(CCVars.AmbienceVolume) * 100f / ContentAudioSystem.AmbienceMultiplier) < 0.01f;
            var isAmbientMusicVolumeSame =
                Math.Abs(AmbientMusicVolumeSlider.Value - _cfg.GetCVar(CCVars.AmbientMusicVolume) * 100f / ContentAudioSystem.AmbientMusicMultiplier) < 0.01f;
            var isLobbyVolumeSame =
                Math.Abs(LobbyVolumeSlider.Value - _cfg.GetCVar(CCVars.LobbyMusicVolume) * 100f / ContentAudioSystem.LobbyMultiplier) < 0.01f;
            var isInterfaceVolumeSame =
                Math.Abs(InterfaceVolumeSlider.Value - _cfg.GetCVar(CCVars.InterfaceVolume) * 100f / ContentAudioSystem.InterfaceMultiplier) < 0.01f;
            var isTtsVolumeSame =
                Math.Abs(TtsVolumeSlider.Value - _cfg.GetCVar(CCCVars.TTSVolume) * 100f / ContentAudioSystem.TtsMultiplier) < 0.01f;
            /* here >> */
            var isTtsRadioVolumeSame =
                Math.Abs(TtsRadioVolumeSlider.Value - _cfg.GetCVar(CCCVars.TTSRadioVolume) * 100f / ContentAudioSystem.TtsMultiplier) < 0.01f;
            var isTtsAnnounceVolumeSame =
                Math.Abs(TtsAnnounceVolumeSlider.Value - _cfg.GetCVar(CCCVars.TTSAnnounceVolume) * 100f / ContentAudioSystem.TtsMultiplier) < 0.01f;
            /* << */

            var isAmbientSoundsSame = (int)AmbienceSoundsSlider.Value == _cfg.GetCVar(CCVars.MaxAmbientSources);
            var isLobbySame = LobbyMusicCheckBox.Pressed == _cfg.GetCVar(CCVars.LobbyMusicEnabled);
            var isRestartSoundsSame = RestartSoundsCheckBox.Pressed == _cfg.GetCVar(CCVars.RestartSoundsEnabled);
            var isEventSame = EventMusicCheckBox.Pressed == _cfg.GetCVar(CCVars.EventMusicEnabled);
            var isAdminSoundsSame = AdminSoundsCheckBox.Pressed == _cfg.GetCVar(CCVars.AdminSoundsEnabled);
            var isAHelpSoundsSame = AHelpSoundsCheckBox.Pressed == _cfg.GetCVar(CCVars.AHelpSoundsEnabled);
            var isEverythingSame = isMasterVolumeSame && isMidiVolumeSame && isAHelpVolumeSame && isAHelpSoundsSame && isAmbientVolumeSame && isAmbientMusicVolumeSame && isAmbientSoundsSame && isLobbySame && isRestartSoundsSame && isEventSame && isAdminSoundsSame && isLobbyVolumeSame && isTtsRadioVolumeSame;
            isEverythingSame = isEverythingSame && isTtsVolumeSame && isTtsAnnounceVolumeSame; // Corvax-TTS
            ApplyButton.Disabled = isEverythingSame;
            ResetButton.Disabled = isEverythingSame;
            MasterVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", MasterVolumeSlider.Value / 100));
            MidiVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", MidiVolumeSlider.Value / 100));
            AmbientMusicVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", AmbientMusicVolumeSlider.Value / 100));
            AmbienceVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", AmbienceVolumeSlider.Value / 100));
            AHelpVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", AHelpVolumeSlider.Value / 100));
            LobbyVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", LobbyVolumeSlider.Value / 100));
            InterfaceVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", InterfaceVolumeSlider.Value / 100));
            TtsVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", TtsVolumeSlider.Value / 100)); // Corvax-TTS
            TtsRadioVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", TtsRadioVolumeSlider.Value / 100)); // Corvax-TTS
            TtsAnnounceVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", TtsAnnounceVolumeSlider.Value / 100));
            AmbienceSoundsLabel.Text = ((int) AmbienceSoundsSlider.Value).ToString();
        }
    }
}
