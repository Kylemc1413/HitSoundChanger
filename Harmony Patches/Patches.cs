using System.Collections.Generic;
using System.Linq;

using HarmonyLib;

using UnityEngine;


namespace HitSoundChanger.Harmony_Patches {


    [HarmonyPatch(typeof(NoteCutSoundEffect))]
    [HarmonyPatch("Awake", MethodType.Normal)]
    class BadCutSoundPatch {

        public static void Prefix(ref AudioClip[] ____badCutSoundEffectAudioClips) {
            if (Plugin.originalBadSounds == null) {
                Plugin.originalBadSounds = new List<AudioClip>();
                Plugin.originalBadSounds.AddRange(____badCutSoundEffectAudioClips);
            }

            if (Plugin.currentHitSound.badHitSoundEffects == null) {
                ____badCutSoundEffectAudioClips = Plugin.originalBadSounds.ToArray();
            }
            else {
                ____badCutSoundEffectAudioClips = Plugin.currentHitSound.badHitSoundEffects;
            }
        }
    }

    [HarmonyPatch(typeof(NoteCutSoundEffectManager))]
    [HarmonyPatch("Start", MethodType.Normal)]
    class HitSoundsPatch {

        public static void Prefix(ref AudioClip[] ____longCutEffectsAudioClips, ref AudioClip[] ____shortCutEffectsAudioClips) {
            if (Plugin.originalLongSounds == null) {
                Plugin.originalLongSounds = new List<AudioClip>();
                Plugin.originalLongSounds.AddRange(____longCutEffectsAudioClips);
            }

            if (Plugin.originalShortSounds == null) {
                Plugin.originalShortSounds = new List<AudioClip>();
                Plugin.originalShortSounds.AddRange(____shortCutEffectsAudioClips);
            }

            if (Plugin.currentHitSound.longHitSoundEffects == null || Plugin.currentHitSound.shortHitSoundEffects == null) {
                ____longCutEffectsAudioClips = Plugin.originalLongSounds.ToArray();
                ____shortCutEffectsAudioClips = Plugin.originalShortSounds.ToArray();
            }
            else {
                ____longCutEffectsAudioClips = Plugin.currentHitSound.longHitSoundEffects;
                ____shortCutEffectsAudioClips = Plugin.currentHitSound.shortHitSoundEffects;
            }
        }
    }

    [HarmonyPatch(typeof(GameNoteController))]
    [HarmonyPatch("NoteDidPassMissedMarker", MethodType.Normal)]
    class MissSoundsPatch {

        static AudioSource[] audioSources;
        static float volumeMultiplier = 0.6f;

        public static void Postfix() {
            if (Plugin.currentHitSound.missSoundEffect != null) {
                if (audioSources == null) {
                    audioSources = new AudioSource[64];
                    for (int i = 0; i < audioSources.Length; i++) {
                        audioSources[i] = new GameObject($"MissSoundEffect {i}").AddComponent<AudioSource>();
                        audioSources[i].clip = Plugin.currentHitSound.missSoundEffect;
                        audioSources[i].volume = volumeMultiplier;
                        GameObject.DontDestroyOnLoad(audioSources[i]);
                    }
                }
                if (audioSources.FirstOrDefault().clip != Plugin.currentHitSound.missSoundEffect) {
                    foreach (AudioSource audioSource in audioSources) {
                        audioSource.clip = Plugin.currentHitSound.missSoundEffect;
                    }
                }
                if (audioSources.FirstOrDefault().volume != volumeMultiplier) {
                    foreach (AudioSource audioSource in audioSources) {
                        audioSource.volume = volumeMultiplier;
                    }
                }
                audioSources.First(x => !x.isPlaying).Play();
            }
        }
    }
}
