using System.Collections.Generic;
using System.Linq;

using HarmonyLib;

using UnityEngine;


namespace HitSoundChanger.HarmonyPatches {


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

        public static List<AudioSource> audioSources = new List<AudioSource>();
        public static float volumeMultiplier = 1;

        public static void Postfix() {
            if (Plugin.currentHitSound.missSoundEffect != null) {
                AudioSource activeAudioSource = audioSources.FirstOrDefault(x => !x.isPlaying);
                if (activeAudioSource == null) {
                    activeAudioSource = new GameObject("MissSoundEffect").AddComponent<AudioSource>();
                    Object.DontDestroyOnLoad(activeAudioSource.gameObject);
                    audioSources.Add(activeAudioSource);
                }
                activeAudioSource.clip = Plugin.currentHitSound.missSoundEffect;
                activeAudioSource.volume = volumeMultiplier * 0.15f;
                activeAudioSource.Play();
            }
        }
    }
}
