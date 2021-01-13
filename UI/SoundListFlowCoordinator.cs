using System;

using BeatSaberMarkupLanguage;

using HMUI;


namespace HitSoundChanger.UI {


    class SoundListFlowCoordinator : FlowCoordinator {

        private SoundListView _soundListView;

        public void Awake() {
            if (_soundListView == null) {
                _soundListView = BeatSaberUI.CreateViewController<SoundListView>();
            }
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling) {
            try {
                if (firstActivation) {

                    SetTitle("Hit Sounds");
                    showBackButton = true;
                    ProvideInitialViewControllers(_soundListView);
                }
                if (addedToHierarchy) {

                }
            }
            catch (Exception ex) {
                Plugin.Logger.Error(ex);
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController) {
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this, null, ViewController.AnimationDirection.Horizontal);
        }
    }
}
