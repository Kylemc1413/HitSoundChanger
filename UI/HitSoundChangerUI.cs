using System;

using BeatSaberMarkupLanguage.MenuButtons;

using UnityEngine;


namespace HitSoundChanger.UI {


    class HitSoundChangerUI : MonoBehaviour {

        internal SoundListView _soundListViewController;
        internal SoundListFlowCoordinator _soundListFlow;
        public static HitSoundChangerUI _instance;

        internal static void OnLoad() {
            if (_instance != null) {
                return;
            }

            new GameObject("HitSoundChangerUI").AddComponent<HitSoundChangerUI>();
        }

        private void Awake() {
            _instance = this;
            try {
            }
            catch (Exception ex) {
                Plugin.Logger.Error($"{ex.Message}\n{ex.StackTrace}");
            }
            GameObject.DontDestroyOnLoad(this);
            CreateHitSoundButton();
        }

        private void CreateHitSoundButton() {
            Plugin.Logger.Debug("Adding HitSounds button");
            MenuButtons.instance.RegisterButton(new MenuButton("HitSounds", "Change HitSounds Here!", HitSoundButtonPressed, true));

        }

        internal void ShowSoundListFlow() {
            if (_soundListFlow == null)
                _soundListFlow = BeatSaberMarkupLanguage.BeatSaberUI.CreateFlowCoordinator<SoundListFlowCoordinator>();
            BeatSaberMarkupLanguage.BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinatorOrAskForTutorial(_soundListFlow);
        }

        private void HitSoundButtonPressed() {
            ShowSoundListFlow();
        }
    }
}
